using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    public class NetworkManager : MonoBehaviour
    {
        //If not master then client
        public bool IsMaster;
        public static NetworkManager singleton;

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
            if (singleton == null)
            {
                resourcesManager = Resources.Load("ResourcesManager") as ResourcesManager;
                singleton = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        #region My Calls
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

        void CreateCardClient_Call(string cardId, int instanceId, int photonId)
        {
            Card card = CreateCardClient(cardId, instanceId);
            if(card != null)
            {
                MultiplayerHolder holder = GetMultiplayerHolder(photonId);
                holder.RegisterCard(card);
            }
        }

        Card CreateCardMaster(string cardId)
        {
            Card card = resourcesManager.GetCardInstance(cardId);
            card.InstanceId = cardInstanceId;
            cardInstanceId++;
            return card;
        }

        Card CreateCardClient(string cardId, int instanceId)
        {
            Card card = resourcesManager.GetCardInstance(cardId);
            card.InstanceId = instanceId;
            return card; 
        }

        #endregion
        #region Photon Callbacks
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
