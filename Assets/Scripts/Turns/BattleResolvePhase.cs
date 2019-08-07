﻿using UnityEngine;
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
            //PlayerHolder player = Settings.gameManager.CurrentPlayer;
            //PlayerHolder enemy = Settings.gameManager.GetOpponentOf(player);

            //if (player.AttackingCards.Count == 0)
            //{
            //    return true;
            //}

            ////Dictionary<CardInstance,BlockInstance> blockInstanceDictionary = Settings.gameManager.GetBlockInstances();


            //foreach (CardInstance attackingInstance in player.AttackingCards)
            //{
            //    bool AttackerDied = false;
            //    Card card = attackingInstance.visual.card;
            //    CardProperty attackerAttackProperty = card.GetProperty(AttackElement);
            //    CardProperty AttackerDefenseProperty = card.GetProperty(DefenseElement);
            //    if (attackerAttackProperty == null) { Debug.Log("Attacking card has no attack property"); continue; }

            //    int attackValue = attackerAttackProperty.intValue;

            //    BlockInstance blockInstance = Settings.gameManager.GetBlockInstanceForAttacker(attackingInstance);
            //    if(blockInstance != null)
            //    {
            //        Debug.Log("Block Instance not null");

            //        foreach (CardInstance blockerCardInstance in blockInstance.Blockers)
            //        {
            //            CardProperty blockerDefenseProperty = blockerCardInstance.visual.card.GetProperty(DefenseElement);
            //            CardProperty blockerAttackProperty = blockerCardInstance.visual.card.GetProperty(AttackElement);

            //            if (blockerDefenseProperty == null)
            //            {
            //                Debug.LogWarning("Block Property is Null");
            //            }
            //            attackValue -= blockerDefenseProperty.intValue;

            //            if(blockerDefenseProperty.intValue <= attackerAttackProperty.intValue)
            //            {
            //                //blocker dies
            //                blockerCardInstance.CardInstanceToDiscard();
            //            }

            //            if(AttackerDefenseProperty.intValue <= blockerAttackProperty.intValue)
            //            {
            //                AttackerDied = true;
            //                //attacker dies
            //                attackingInstance.CardInstanceToDiscard();
            //            }

            //        }

            //    }

            //    if (attackValue < 0)
            //    {
            //        attackValue = 0;
            //    }

            //    if(!AttackerDied)
            //    {
            //        player.DropCard(attackingInstance, false);
            //        player.CurrentCardHolder.SetCardDown(attackingInstance);
            //        attackingInstance.SetExhausted(true);
            //    }

            //    enemy.TakeDamage(attackValue);
            //}

            //Settings.gameManager.ClearBlockInstances();
            //player.AttackingCards.Clear();
            if (forceExit)
            {
                forceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
            IsInit = false;
        }

        public override void OnStartPhase()
        {
            if (!IsInit)
            {
                IsInit = true;
                MultiplayerManager.Singleton.SetBattleResolvePhase();
            }
        }


        //BlockInstance GetBlockInstanceForAttacker(CardInstance attacker, Dictionary<CardInstance, BlockInstance> blockInstanceDictionary)
        //{
        //    BlockInstance blockInstance = null;
        //    blockInstanceDictionary.TryGetValue(attacker, out blockInstance);
        //    return blockInstance;
        //}
    }
}
