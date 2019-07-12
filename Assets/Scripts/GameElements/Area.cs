using UnityEngine;
using UnityEditor;

namespace PL.GameElements
{
    public class Area : MonoBehaviour
    {
        public AreaLogic logic;

        public void OnDrop()
        {
            logic.Execute();
        }
    }
}