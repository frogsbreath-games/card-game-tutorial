using UnityEngine;
using System.Collections;

namespace PL {
    [CreateAssetMenu(menuName = "Turns/Battle Resolve Phase")]
    public class BattleResolvePhase : Phase
    {
        public CardElement AttackElement;
        public CardElement DefenseElement;

        public override bool IsComplete()
        {
            PlayerHolder player = Settings.gameManager.CurrentPlayer;
            PlayerHolder enemy = Settings.gameManager.GetOpponentOf(player);

            if (player.AttackingCards.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < player.AttackingCards.Count; i++)
            {
                CardInstance instance = player.AttackingCards[i];
                Card card = instance.visual.card;
                CardProperty attackProperty = card.GetProperty(AttackElement);
                if (attackProperty == null) { Debug.Log("Attacking card has no attack property"); continue; }

                enemy.TakeDamage(attackProperty.intValue);
                player.DropCard(instance, false);
                player.CurrentCardHolder.SetCardDown(instance);
                instance.SetExhausted(true);
            }

            player.AttackingCards.Clear();

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
