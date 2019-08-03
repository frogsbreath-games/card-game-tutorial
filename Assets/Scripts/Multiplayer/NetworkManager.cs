using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SO;
using System.Linq;

namespace PL
{
    public class NetworkManager : Photon.PunBehaviour
    {
        //If not master then client
        public static bool IsMaster;
        public static NetworkManager Singleton;

        public StringVariable LoggerTextVariable;

        public GameEvent OnConnected;
        public GameEvent OnFailedConnect;
        public GameEvent LoggerUpdated;
        public GameEvent WaitingForPlayer;

        int cardInstanceId;

        List<MultiplayerHolder> MultiplayerHolders = new List<MultiplayerHolder>();

        public MultiplayerHolder GetMultiplayerHolder(int photonId)
        {
            foreach (MultiplayerHolder holder in MultiplayerHolders)
            {
                if(holder.OwnerId == photonId)
                {
                    return holder;
                }
            }
            return null;
        }

        public Card GetCard(int instanceId, int OwnerId)
        {
            MultiplayerHolder holder = GetMultiplayerHolder(OwnerId);
            return holder.GetCard(instanceId);
        }

        ResourcesManager resourcesManager;

        private void Awake()
        {
            if (Singleton == null)
            {
                resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
                Singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        private void Start()
        {
            resourcesManager.Init();
            PhotonNetwork.autoCleanUpPlayerObjects = false;
            PhotonNetwork.autoJoinLobby = false;
            PhotonNetwork.automaticallySyncScene = false;
            Init();
        }

        private void Init()
        {
            PhotonNetwork.ConnectUsingSettings("1");
            LoggerTextVariable.value = "Connecting...";
            LoggerUpdated.Raise();
        }

        #region My Calls
        public void OnStartGame()
        {
            JoinRandomRoom();
        }
        void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();
        }
            
        void CreateRoom()
        {
            RoomOptions room = new RoomOptions();
            room.MaxPlayers = 2;
            PhotonNetwork.CreateRoom(RandomString(256), room, TypedLobby.Default);

        }

        private System.Random random = new System.Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789abcdefghijklmno";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        //Master Only
        public void PlayerJoin(int photonId, string[] cards)
        {
            MultiplayerHolder holder = new MultiplayerHolder();
            holder.OwnerId = photonId;

            foreach (string cardId in cards)
            {
                Card card = CreateCardMaster(cardId);
                if (card != null)
                {
                    holder.RegisterCard(card);
                    //RPC CreateCardCall this is where the new created starting cards will happen
                }
            }
        }

        //void CreateCardClient_Call(string cardId, int instanceId, int photonId)
        //{
        //    Card card = CreateCardClient(cardId, instanceId);
        //    if(card != null)
        //    {
        //        MultiplayerHolder holder = GetMultiplayerHolder(photonId);
        //        holder.RegisterCard(card);
        //    }
        //}

        Card CreateCardMaster(string cardId)
        {
            Card card = resourcesManager.GetCardInstance(cardId);
            card.InstanceId = cardInstanceId;
            cardInstanceId++;
            return card;
        }

        //Card CreateCardClient(string cardId, int instanceId)
        //{
        //    Card card = resourcesManager.GetCardInstance(cardId);
        //    card.InstanceId = instanceId;
        //    return card; 
        //}

        #endregion
        #region Photon Callbacks
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();
            LoggerTextVariable.value = "Connected";
            LoggerUpdated.Raise();
            OnConnected.Raise();
        }

        public override void OnFailedToConnectToPhoton(DisconnectCause cause)
        {
            base.OnFailedToConnectToPhoton(cause);
            LoggerTextVariable.value = "Failed to connect";
            LoggerUpdated.Raise();
            OnFailedConnect.Raise();
        }

        public override void OnCreatedRoom()
        {
            base.OnCreatedRoom();
            IsMaster = true;
        }

        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();
            LoggerTextVariable.value = "Waiting for Player";
            LoggerUpdated.Raise();
            WaitingForPlayer.Raise();
        }

        public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
        {
            if (IsMaster)
            {
                if(PhotonNetwork.playerList.Length > 1)
                {
                    //start match
                    LoggerTextVariable.value = "Ready for match";
                    LoggerUpdated.Raise();
                    PhotonNetwork.room.IsOpen = false;

                    PhotonNetwork.Instantiate("MultiplayerManager", Vector3.zero, Quaternion.identity, 0);
                }
            }
        }

        public void LoadGameScene()
        {
            SessionManager.Singleton.LoadGameLevel(OnGameSceneLoaded);
        }

        void OnGameSceneLoaded()
        {
            //Responsible for getting the data from network players and assigning them on player holders
            MultiplayerManager.Singleton.CheckPlayerCount = true;
        }

        public override void OnDisconnectedFromPhoton()
        {
            base.OnDisconnectedFromPhoton();
        }

        public override void OnLeftRoom()
        {
            base.OnLeftRoom();
        }

        public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
        {
            base.OnPhotonRandomJoinFailed(codeAndMsg);
            CreateRoom();
        }
        #endregion
        #region RPCs
        #endregion
    }

    public class MultiplayerHolder
    {
        public int OwnerId;
        public Dictionary<int, Card> Cards = new Dictionary<int, Card>();

        public void RegisterCard(Card card)
        {
            Cards.Add(card.InstanceId, card);
        }

        public Card GetCard(int instanceId)
        {
            Card card = null;
            Cards.TryGetValue(instanceId, out card);
            return card;
        }
    }
}
