using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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

            //Dictionary<CardInstance,BlockInstance> blockInstanceDictionary = Settings.gameManager.GetBlockInstances();


            foreach (CardInstance attackingInstance in player.AttackingCards)
            {
                Card card = attackingInstance.visual.card;
                CardProperty attackProperty = card.GetProperty(AttackElement);
                if (attackProperty == null) { Debug.Log("Attacking card has no attack property"); continue; }

                int attackValue = attackProperty.intValue;

                BlockInstance blockInstance = Settings.gameManager.GetBlockInstanceForAttacker(attackingInstance);
                if(blockInstance != null)
                {
                    foreach (CardInstance blockerCardInstance in blockInstance.Blockers)
                    {
                        CardProperty defenseProperty = blockerCardInstance.visual.card.GetProperty(DefenseElement);
                        if(defenseProperty == null)
                        {
                            Debug.LogWarning("Block Property is Null");
                        }
                        attackValue -= defenseProperty.intValue;
                        
                        if(defenseProperty.intValue <= attackValue)
                        {
                            //blocker dies
                            blockerCardInstance.CardInstanceToDiscard();
                        }
                    }

                }

                if (attackValue < 0)
                {
                    attackValue = 0;
                    //Attacker dies
                    attackingInstance.CardInstanceToDiscard();
                }

                enemy.TakeDamage(attackValue);
                player.DropCard(attackingInstance, false);
                player.CurrentCardHolder.SetCardDown(attackingInstance);
                attackingInstance.SetExhausted(true);
            }

            Settings.gameManager.ClearBlockInstances();
            player.AttackingCards.Clear();

            return true;
        }

        public override void OnEndPhase()
        {

        }

        public override void OnStartPhase()
        {
            
        }


        BlockInstance GetBlockInstanceForAttacker(CardInstance attacker, Dictionary<CardInstance, BlockInstance> blockInstanceDictionary)
        {
            BlockInstance blockInstance = null;
            blockInstanceDictionary.TryGetValue(attacker, out blockInstance);
            return blockInstance;
        }
    }
}
