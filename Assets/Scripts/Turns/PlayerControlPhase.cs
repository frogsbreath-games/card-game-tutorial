using UnityEngine;
using System.Collections;
namespace PL
{
    [CreateAssetMenu(menuName ="Turns/Player Control Phase")]
    public class PlayerControlPhase : Phase
    {
        public GameStates.State playerControlState;

        public override bool IsComplete()
        {
            if (forceExit)
            {
                forceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
            if (IsInit)
            {
                Settings.gameManager.SetState(null);
                IsInit = false;
            }
        }

        public override void OnStartPhase()
        {
            if (!IsInit)
            {
                Settings.gameManager.SetState(playerControlState);
                Settings.gameManager.onPhaseChange.Raise();
                IsInit = true;
            }
        }
    }
}
