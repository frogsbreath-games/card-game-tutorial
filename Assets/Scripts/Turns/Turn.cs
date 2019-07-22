using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PL
{
    [CreateAssetMenu(menuName ="Turns/Turn")]
    public class Turn : ScriptableObject
    {
        public string TurnName;
        public PlayerHolder Player;
        //Always stored as zero when game starts
        [System.NonSerialized]
        int index;
        public PhaseVariable currentPhase;

        public Phase[] phases;

        public PlayerAction[] TurnStartActions;

        public void OnTurnStart()
        {
            if (TurnStartActions == null)
            {
                return;
            }

            foreach (PlayerAction action in TurnStartActions)
            {
                action.Execute(Player);
            }
        }

        public bool Execute()
        {
            bool result = false;

            currentPhase.value = phases[index];
            phases[index].OnStartPhase();

            bool phaseComplete = phases[index].IsComplete();

            if (phaseComplete)
            {
                phases[index].OnEndPhase();

                index++;
                if(index > phases.Length - 1)
                {
                    index = 0;
                    result = true;
                }

            }

            return result;
        }

        public void EndCurrentPhase()
        {
            phases[index].forceExit = true;
        }
    }
}
