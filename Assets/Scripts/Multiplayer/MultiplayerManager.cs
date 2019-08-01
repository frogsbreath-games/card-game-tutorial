using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    public class MultiplayerManager : Photon.MonoBehaviour
    {

        #region variables
        List<NetworkPrint> Players = new List<NetworkPrint>();
        public static MultiplayerManager Singleton;
        NetworkPrint LocalPlayer;

        public PlayerHolder LocalPlayerHolder;
        public PlayerHolder ClientPlayerHolder;

        GameManager gameManager
        {
            get { return GameManager.Singleton; }
        }

        public bool CheckPlayerCount;
        bool GameStarted;

        Transform MultiplayerReferences;
        #endregion
        #region init
        //Responsible for sending events to each of the players
        //Get instantiated by everyone that is connected and syncs across
        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            MultiplayerReferences = new GameObject("References").transform;
            DontDestroyOnLoad(MultiplayerReferences.gameObject);
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);

            InstantiateNetworkPrint();
            NetworkManager.Singleton.LoadGameScene();
        }

        void InstantiateNetworkPrint()
        {
            //Network print responsible for adding and assigning network print for the player
            PlayerProfile player = Resources.Load("PlayerProfile") as PlayerProfile;
            object[] data = new object[1];
            data[0] = player.CardIds;

            PhotonNetwork.Instantiate("NetworkPrint", Vector3.zero, Quaternion.identity, 0, data);
        }
        #endregion

        #region Tick Update
        private void Update()
        {
            if (!GameStarted)
            {
                if(Players.Count > 1 && CheckPlayerCount)
                {
                    GameStarted = true;
                    StartMatch();
                }
            }

        }
        #endregion

        #region Starting Match
        public void StartMatch()
        {
            GameManager manager = GameManager.Singleton;

            foreach(NetworkPrint player in Players)
            {
                if (player.IsLocal)
                {
                    LocalPlayerHolder.PhotonId = player.PhotonId;
                    LocalPlayerHolder.AllCards.Clear();
                    LocalPlayerHolder.AllCards.AddRange(player.GetStartingCardIds());
                }
                else
                {
                    ClientPlayerHolder.PhotonId = player.PhotonId;
                    ClientPlayerHolder.AllCards.Clear();
                    ClientPlayerHolder.AllCards.AddRange(player.GetStartingCardIds());
                }
            }

            if (NetworkManager.IsMaster)
            {
                photonView.RPC("RPC_InitGame", PhotonTargets.All, 1);
            }
        }

        public void AddPlayer(NetworkPrint networkPrint)
        {
            if (networkPrint.IsLocal)
            {
                LocalPlayer = networkPrint;
            }
            Players.Add(networkPrint);
            networkPrint.transform.parent = MultiplayerReferences;
        }

        [PunRPC]
        public void RPC_InitGame(int startingPlayerId)
        {
            gameManager.IsMultiplayer = true;
            gameManager.GameInit(startingPlayerId);
            //First player to join is the first to play
        }

        NetworkPrint GetPlayer(int photonId)
        {
            foreach (NetworkPrint player in Players)
            {
                if(player.PhotonId == photonId)
                {
                    return player;
                }
            }
            return null;
        }
        #endregion

        #region End Turn
        public void PlayerEndsTurn(int playerPhotonId)
        {
            photonView.RPC("RPC_PlayerEndsTurn", PhotonTargets.MasterClient, playerPhotonId);
        }

        [PunRPC]
        public void RPC_PlayerEndsTurn(int playerPhotonId)
        {
            if (playerPhotonId == gameManager.CurrentPlayer.PhotonId)
            {
                //gameManager.ChangeCurrentTurn(playerPhotonId);
                if (NetworkManager.IsMaster)
                {
                    int photonId = gameManager.GetPhotonIdForNextPlayer();
                    photonView.RPC("RPC_PlayerStartsTurn", PhotonTargets.All, photonId);
                }
            }
        }

        [PunRPC]
        public void RPC_PlayerStartsTurn(int playerPhotonId)
        {
            gameManager.ChangeCurrentTurn(playerPhotonId);
        }
        #endregion
    }
}
