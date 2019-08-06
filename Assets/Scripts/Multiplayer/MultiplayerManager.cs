using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    public class MultiplayerManager : Photon.MonoBehaviour
    {

        #region variables
        #region Player Management
        List<NetworkPrint> Players = new List<NetworkPrint>();
        NetworkPrint LocalPlayer;

        NetworkPrint GetPlayer(int photonId)
        {
            foreach (NetworkPrint player in Players)
            {
                if (player.PhotonId == photonId)
                {
                    return player;
                }
            }
            return null;
        }
        #endregion

        public static MultiplayerManager Singleton;

        public MainDataHolder DataHolder;

        //public PlayerHolder LocalPlayerHolder;
        //public PlayerHolder ClientPlayerHolder;

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
            ResourcesManager resourcesManager = gameManager.ResourcesManager;

            foreach (NetworkPrint player in Players)
            {

                if (player.IsLocal)
                {
                    player.PlayerHolder = gameManager.LocalPlayer;
                }
                else
                {
                    player.PlayerHolder = gameManager.ClientPlayer;
                }

                player.PlayerHolder.PhotonId = player.PhotonId;
            }

            if (NetworkManager.IsMaster)
            {
                List<int> playerIds = new List<int>();
                List<int> cardInstanceIds = new List<int>();
                List<string> cardNames = new List<string>();
                foreach (NetworkPrint player in Players)
                {
                    foreach (string id in player.GetStartingCardIds())
                    {
                        Card card = resourcesManager.GetCardInstance(id);
                        playerIds.Add(player.PhotonId);
                        cardInstanceIds.Add(card.InstanceId);
                        cardNames.Add(id);
                    }
                }

                for (int i = 0; i < cardInstanceIds.Count; i++)
                {
                    photonView.RPC("RPC_PlayerCreateCard", PhotonTargets.All, playerIds[i], cardInstanceIds[i], cardNames[i]);
                }
          
                photonView.RPC("RPC_InitGame", PhotonTargets.All, 1);
            }
        }

        [PunRPC]
        public void RPC_PlayerCreateCard(int photonId, int cardId, string cardName)
        {
            Card card = gameManager.ResourcesManager.GetCardInstance(cardName);
            card.InstanceId = cardId;

            NetworkPrint player = GetPlayer(photonId);

            player.AddCard(card);

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
        #endregion

        #region End Turn
        public void PlayerDrawsCardFromDeck(PlayerHolder player)
        {
            NetworkPrint p = GetPlayer(player.PhotonId);

            Card c = p.DeckCards[0];
            PlayerAttemptsToPlayCard(c.InstanceId, p.PhotonId, CardOperation.pickCardFromDeck);
        }

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

        #region Card Check
    
        public void PlayerAttemptsToPlayCard(int cardInstance, int photonId, CardOperation operation)
        {
            photonView.RPC("RPC_PlayerAttemptsToPlayCard", PhotonTargets.MasterClient, cardInstance, photonId, operation);
        }

        [PunRPC]
        public void RPC_PlayerAttemptsToPlayCard(int cardInstance, int photonId, CardOperation operation)
        {
            if (!NetworkManager.IsMaster)
            {
                return;
            }

            bool hasCard = PlayerOwnsCard(cardInstance, photonId);

            if (hasCard)
            {
                photonView.RPC("RPC_PlayerUsesCard", PhotonTargets.All, cardInstance, photonId, operation);
            }
        }


        bool PlayerOwnsCard(int cardInstance, int photonId)
        {
            NetworkPrint player = GetPlayer(photonId);
            Card card = player.GetCard(cardInstance);

            return card != null;
        }
        #endregion

        #region CardOperations
        public enum CardOperation
        {
            dropResourcesCard,pickCardFromDeck,dropCreatureCard,attackWithCreature
        }

        [PunRPC]
        public void RPC_PlayerUsesCard(int instanceId, int photonId, CardOperation operation)
        {
            NetworkPrint player = GetPlayer(photonId);
            Card card = player.GetCard(instanceId);
            //check if active player can play card
            bool canPlay = player.PlayerHolder.CanPlayCard(card);

            switch (operation)
            {
                case CardOperation.dropResourcesCard:
                    Settings.SetParentForCard(card.GameInstance.transform, player.PlayerHolder.CurrentCardHolder.ResourceGrid.value);
                    player.PlayerHolder.AddResourceCard(card.GameInstance.gameObject);
                    card.GameInstance.currentLogic = DataHolder.PlayedCardLogic;
                    card.GameInstance.gameObject.SetActive(true);
                    break;
                case CardOperation.pickCardFromDeck:
                    GameObject cardObject = Instantiate(DataHolder.CardPrefab) as GameObject;
                    CardVisual visual = cardObject.GetComponent<CardVisual>();
                    visual.LoadCard(card);
                    card.GameInstance = cardObject.GetComponent<CardInstance>();
                    card.GameInstance.Owner = player.PlayerHolder;
                    card.GameInstance.currentLogic = DataHolder.HandCardLogic;
                    //need player holder access on the network print
                    Settings.SetParentForCard(cardObject.transform, player.PlayerHolder.CurrentCardHolder.HandGrid.value.transform);
                    player.PlayerHolder.HandCards.Add(card.GameInstance);
                    player.DeckCards.RemoveAt(0);
                    break;
                case CardOperation.dropCreatureCard:

                    if (canPlay)
                    {
                        Settings.PlayCreatureCard(card.GameInstance.transform, player.PlayerHolder.CurrentCardHolder.PlayedGrid.value.transform, card.GameInstance);
                        card.GameInstance.currentLogic = DataHolder.PlayedCardLogic;
                    }
                    else
                    {
                        Settings.RegisterEvent(player.PlayerHolder.Username + " not enough resource to play card.", player.PlayerHolder.PlayerColor);
                    }

                    card.GameInstance.gameObject.SetActive(true);
                    break;
                case CardOperation.attackWithCreature:
                    if (player.PlayerHolder.AttackingCards.Contains(card.GameInstance))
                    {
                        player.PlayerHolder.AttackingCards.Remove(card.GameInstance);
                        player.PlayerHolder.CurrentCardHolder.SetCardDown(card.GameInstance);
                    }
                    else if (card.GameInstance.CanAttack())
                    {
                        player.PlayerHolder.AttackingCards.Add(card.GameInstance);
                        player.PlayerHolder.CurrentCardHolder.SetCardOnBattleLine(card.GameInstance);
                        Settings.RegisterEvent($"{card.name} is attacking.", player.PlayerHolder.PlayerColor);
                    }
                    break;
                default:
                    break;
            }

        }
        #endregion

        #region MultipleCard Operations
        #region Reset Creatures
        public void PlayerWantsToResetFlatFootedCards(int photonId)
        {
            photonView.RPC("RPC_ResetFlatFootedCardsForPlayer_Master", PhotonTargets.MasterClient, photonId);
        }
        [PunRPC]
        public void RPC_ResetFlatFootedCardsForPlayer_Master(int photonId)
        {
            NetworkPrint player = GetPlayer(photonId);
            if(gameManager.Turns[gameManager.TurnIndex].Player == player.PlayerHolder)
            {
                photonView.RPC("RPC_ResetFlatFootedCardsForPlayer", PhotonTargets.All, photonId);
            }
        }

        [PunRPC]
        public void RPC_ResetFlatFootedCardsForPlayer(int photonId)
        {
            NetworkPrint player = GetPlayer(photonId);
            foreach (CardInstance card in player.PlayerHolder.PlayedCards)
            {
                if (card.IsExhausted)
                {
                    card.SetExhausted(false);
                }
            }
        }
        #endregion
        #region Reset Resource
        public void PlayerWantsToResetResourceCards(int photonId)
        {
            photonView.RPC("RPC_ResetResourceCardsForPlayer_Master", PhotonTargets.MasterClient, photonId);
        }
        [PunRPC]
        public void RPC_ResetResourceCardsForPlayer_Master(int photonId)
        {
            NetworkPrint player = GetPlayer(photonId);
            if (gameManager.Turns[gameManager.TurnIndex].Player == player.PlayerHolder)
            {
                photonView.RPC("RPC_ResetResourceCardsForPlayer", PhotonTargets.All, photonId);
            }
        }
        [PunRPC]
        public void RPC_ResetResourceCardsForPlayer(int photonId)
        {
            NetworkPrint player = GetPlayer(photonId);
            player.PlayerHolder.RefreshPlayerResource();
        }
        #endregion
        #endregion
    }
}
