using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    [CreateAssetMenu(menuName = "Managers/Resources Manager")]
    public class ResourcesManager : ScriptableObject
    {
        public CardElement typeElement;
        public Card[] allCards;

        [System.NonSerialized]
        Dictionary<string, Card> cardDictionary = new Dictionary<string, Card>();

        int CardIndex;

        public void Init()
        {
            CardIndex = 0;
            cardDictionary.Clear();
            for (int i = 0; i < allCards.Length; i++)
            {
                cardDictionary.Add(allCards[i].name, allCards[i]);
            }
        }

        public Card GetCardInstance(string cardId)
        {
            Card card = GetCard(cardId);
            if(card == null) {
                return null;
            }

            Card cardInstance = Instantiate(card);
            cardInstance.name = card.name;
            cardInstance.InstanceId = CardIndex;
            CardIndex++;
            return cardInstance;
        }

        Card GetCard(string cardId)
        {
            Card result = null;
            cardDictionary.TryGetValue(cardId, out result);
            return result;
        }
    }
}
