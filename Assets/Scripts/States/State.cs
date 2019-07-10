using UnityEngine;
using System.Collections;

namespace PL.GameStates
{
    [CreateAssetMenu(menuName = "State")]
    public class State : ScriptableObject
    {
        public Action[] actions;

        public void Tick(float delta)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                actions[i].Execute(delta);
            }
        }
    }
}
