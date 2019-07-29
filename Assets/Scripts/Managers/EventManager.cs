using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public class EventManager : MonoBehaviour
    {
        #region Calls
        public void CardIsDropped(int instanceId, int ownerId)
        {
            Card card = NetworkManager.singleton.GetCard(instanceId, ownerId);
        }

        public void CardIsDrawnFromDeck(int instanceId, int ownerId)
        {
            Card card = NetworkManager.singleton.GetCard(instanceId, ownerId);
        }

        #endregion
    }
}
