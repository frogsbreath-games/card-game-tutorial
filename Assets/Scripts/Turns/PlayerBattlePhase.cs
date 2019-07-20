using UnityEngine;
using System.Collections;
namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Player Battle Phase")]
    public class PlayerBattlePhase : Phase
    {
        public GameStates.State BattleControlState;
        public GameStates.Condition IsBattleValid;

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
                    forceExit = !IsBattleValid.IsValid();
                    Settings.gameManager.SetState(!forceExit ? BattleControlState : null);
                    Settings.gameManager.onPhaseChange.Raise();
                    isInit = true;


            }
        }
    }
}
