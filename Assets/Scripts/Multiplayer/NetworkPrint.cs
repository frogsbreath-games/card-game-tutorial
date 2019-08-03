using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int PhotonId;
        public bool IsLocal;

        public PlayerHolder PlayerHolder;

        string[] CardIds;
        Dictionary<int, Card> PlayerCards = new Dictionary<int, Card>();
        public List<Card> DeckCards = new List<Card>();

        public void AddCard(Card c)
        {
            PlayerCards.Add(c.InstanceId, c);
            DeckCards.Add(c);
        }

        public Card GetCard(int instanceId)
        {
            Card c = null;
            PlayerCards.TryGetValue(instanceId, out c);
            return c;
        }

        public string[] GetStartingCardIds()
        {
            return CardIds;
        }

        void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            PhotonId = photonView.ownerId;
            IsLocal = photonView.isMine;
            object[] data = photonView.instantiationData;
            CardIds = (string[])data[0];

            MultiplayerManager.Singleton.AddPlayer(this);
        }
    }
}
