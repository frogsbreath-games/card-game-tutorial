using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PL
{
    [CreateAssetMenu(menuName ="Turns/Turn")]
    public class Turn : ScriptableObject
    {
        //Always stored as zero when game starts
        [System.NonSerialized]
        int index;

        public Phase[] phases;

        public bool Execute()
        {
            bool result = false;

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
    }
}
