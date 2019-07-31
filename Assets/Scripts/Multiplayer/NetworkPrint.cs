using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public class NetworkPrint : Photon.MonoBehaviour
    {
        public int PhotonId;
        public bool IsLocal;

        string[] CardIds;

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
