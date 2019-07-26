using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Player Block Phase")]
    public class PlayerBlockPhase : Phase
    {
        public PlayerAction ChangeActivePlayer;
        public GameStates.State PlayerBlockState;

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
                GameManager gameManager = Settings.gameManager;
                gameManager.SetState(PlayerBlockState);
                gameManager.onPhaseChange.Raise();
                isInit = true;

                if(gameManager.CurrentPlayer.AttackingCards.Count == 0)
                {
                    forceExit = true;
                    return;
                }

                if (gameManager.EnemyPlayerCardHolder.Player.IsHuman)
                {
                    gameManager.LoadPlayerActive(gameManager.EnemyPlayerCardHolder.Player);
                }
                else
                {
                    //AI autoblocking
                }
            }
        }
    }
}
