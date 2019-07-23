using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace PL
{
    public static class Settings
    {  
        public static GameManager gameManager;

        private static ResourcesManager _resourceManager;

        private static ConsoleHook _consoleManager;

        public static void RegisterEvent(string s, Color color = default)
        {
            if(_consoleManager == null)
            {
                _consoleManager = Resources.Load("ConsoleHook") as ConsoleHook;
            }

            _consoleManager.RegisterEvent(s,color);
        }

        public static ResourcesManager GetResourcesManager()
        {
            if (_resourceManager == null)
            {
                _resourceManager = Resources.Load("ResourcesManager") as ResourcesManager;
                _resourceManager.Init();
            }

            return _resourceManager;
        }

        public static List<RaycastResult> GetUIObjectsUnderMouse()
        {
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            //Raycast to determine what kind of element we are above
            List<RaycastResult> results = new List<RaycastResult>();
            //Holds a list of objects we hit 
            EventSystem.current.RaycastAll(pointerData, results);
            return results;
        }

        public static void PlayCreatureCard(Transform c, Transform p, CardInstance cardInstance)
        {
            SetParentForCard(c, p);
            cardInstance.SetExhausted(true);
            //Execute OnDrop Abilities
            
            gameManager.CurrentPlayer.SpendResource(cardInstance.visual.card.ResourceCost);
            gameManager.CurrentPlayer.DropCard(cardInstance);
        }

        public static void SetParentForCard(Transform c, Transform p)
        {
            c.SetParent(p);
            c.localPosition = Vector3.zero;
            c.localEulerAngles = Vector3.zero;
            c.localScale = Vector3.one;
        }
    }
}
