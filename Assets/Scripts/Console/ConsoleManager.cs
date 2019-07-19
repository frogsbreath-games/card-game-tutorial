using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PL
{
    public class ConsoleManager : MonoBehaviour
    {
        public Transform ConsoleGrid;
        public GameObject Prefab;
        Text[] TextObjects;
        int index;

        public ConsoleHook ConsoleHook;

        private void Awake()
        {
            ConsoleHook.ConsoleManager = this;

            index = 0;
            TextObjects = new Text[5];
            for (int i = 0; i < TextObjects.Length; i++)
            {
                GameObject gameObject = Instantiate(Prefab) as GameObject;
                TextObjects[i] = gameObject.GetComponent<Text>();
                gameObject.transform.SetParent(ConsoleGrid);
            }
        }

        public void RegisterEvent(string s, Color color)
        {
            index++;
            if(index > TextObjects.Length - 1)
            {
                index = 0;
            }

            TextObjects[index].color = color;
            TextObjects[index].text = s;
            TextObjects[index].gameObject.SetActive(true);

        }
    }
}
