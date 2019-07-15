using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PL
{
    [CreateAssetMenu(menuName ="Holders/Player Holder")]
    public class PlayerHolder : ScriptableObject
    {
        public string[] startingCards;
        public SO.TransformVariable handGrid;
        public SO.TransformVariable playedGrid;

        public GameElements.GameElementLogic handLogic;
        public GameElements.GameElementLogic playedLogic;


        [System.NonSerialized]
        public List<CardInstance> handCards = new List<CardInstance>();

        [System.NonSerialized]
        public List<CardInstance> playedCards = new List<CardInstance>();
    }
}
