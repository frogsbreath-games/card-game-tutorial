using UnityEngine;
using System.Collections;
namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Player Battle Phase")]
    public class PlayerBattlePhase : Phase
    {
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
            if (isInit)
            {
                Settings.gameManager.SetState(null);
                isInit = false;
            }
        }

        public override void OnStartPhase()
        {
            if (!isInit)
            {
                Settings.gameManager.SetState(null);
                Settings.gameManager.onPhaseChange.Raise();
                isInit = true;
            }
        }
    }
}
