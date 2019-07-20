using UnityEngine;
using System.Collections;
using PL.GameStates;

namespace PL
{
    [CreateAssetMenu(menuName = "Conditions/Battle Phase Start Check")]
    public class BattlePhaseStartCheck : Condition
    {
        public override bool IsValid()
        {
            GameManager gameManager = GameManager.Singleton;

            if (gameManager.CurrentPlayer.PlayedCards.Count > 0)
            {
                return true;
            }

            return false;
        }
    }
}
