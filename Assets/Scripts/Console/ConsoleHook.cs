using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName ="Console/Hook")]
    public class ConsoleHook : ScriptableObject
    {
        [System.NonSerialized]
        public ConsoleManager ConsoleManager;
        
        public void RegisterEvent(string s, Color color)
        {
            ConsoleManager.RegisterEvent(s, color);
        }
    }
}
