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
            PlayerHolder playerHolder = gameManager.CurrentPlayer;

            int NonExhaustedCreatures = 0;
            for (int i = 0; i < playerHolder.PlayedCards.Count; i++)
            {
                if (!playerHolder.PlayedCards[i].IsExhausted)
                {
                    NonExhaustedCreatures++;
                }
            }

            if (NonExhaustedCreatures > 0)
            {
                return true;
            }

            return false;
        }
    }
}
