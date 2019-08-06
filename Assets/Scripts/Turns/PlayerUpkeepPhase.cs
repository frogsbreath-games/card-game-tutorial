using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Player Upkeep Phase")]
    public class PlayerUpkeepPhase : Phase
    {
        public override bool IsComplete()
        {
            // TODO get player reference from phase
            //MultiplayerManager.Singleton.PlayerWantsToResetResourceCards(GameManager.Singleton.CurrentPlayer.PhotonId);
            return true;
        }

        public override void OnEndPhase()
        {

        }

        public override void OnStartPhase()
        {

        }
    }
}

