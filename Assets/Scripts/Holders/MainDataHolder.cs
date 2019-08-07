using UnityEngine;
using System.Collections;
using SO;

namespace PL
{
    [CreateAssetMenu (menuName ="Holders/Main Data Holder")]
    public class MainDataHolder : ScriptableObject
    {
        public GameElements.GameElementLogic PlayedCardLogic;
        public GameElements.GameElementLogic HandCardLogic;
        public GameObject CardPrefab;
        public CardElement AttackElement;
        public CardElement DefenseElement;

    }
}
