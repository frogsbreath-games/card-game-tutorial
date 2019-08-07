using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PL {
    [CreateAssetMenu(menuName = "Turns/Battle Resolve Phase")]
    public class BattleResolvePhase : Phase
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
