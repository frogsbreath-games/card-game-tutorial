using UnityEngine;
using System.Collections;

namespace PL.GameStates
{
    public abstract class Action : ScriptableObject
    {
        public abstract void Execute(float delta);
    }
}
