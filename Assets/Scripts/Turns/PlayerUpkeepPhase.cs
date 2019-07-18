using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Player Upkeep Phase")]
    public class PlayerUpkeepPhase : Phase
    {
        public override bool IsComplete()
        {
            Settings.gameManager.CurrentPlayer.RefreshPlayerResource();
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

