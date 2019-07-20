using UnityEngine;
using System.Collections;

namespace PL.GameStates
{
    public abstract class Condition : ScriptableObject
    {
        public abstract bool IsValid();
    }
}
