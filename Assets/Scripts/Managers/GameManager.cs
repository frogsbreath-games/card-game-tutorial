using UnityEngine;
using System.Collections;
using PL.GameStates;

namespace PL
{
    public class GameManager : MonoBehaviour
    {
        public State currentState;

        private void Update()
        {
            currentState.Tick(Time.deltaTime);
        }
    }
}