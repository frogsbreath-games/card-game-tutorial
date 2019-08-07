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
                GameManager gameManager = Settings.gameManager;
                gameManager.SetState(PlayerBlockState);
                gameManager.onPhaseChange.Raise();
                IsInit = true;

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
