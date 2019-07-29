using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace PL
{
    [CreateAssetMenu(menuName = "Card")]
    public class Card : ScriptableObject
    {
        [System.NonSerialized]
        public int InstanceId;
        [System.NonSerialized]
        public CardVisual Visual;

        public CardType cardType;
        public int ResourceCost;
        public CardProperty[] cardProperties;

        public CardProperty GetProperty(CardElement element)
        {
            for (int i = 0; i < cardProperties.Length; i++)
            {
                if (cardProperties[i].cardElement == element)
                {
                    return cardProperties[i];
                }
            }

            return null;
        }
    }
}
