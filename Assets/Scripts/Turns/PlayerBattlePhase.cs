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
                    forceExit = !IsBattleValid.IsValid();
                    Settings.gameManager.SetState(!forceExit ? BattleControlState : null);
                    Settings.gameManager.onPhaseChange.Raise();
                    IsInit = true;


            }
        }
    }
}
