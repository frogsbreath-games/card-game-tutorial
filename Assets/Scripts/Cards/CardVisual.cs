using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PL
{
    public class CardVisual : MonoBehaviour
    {

        public Card card;
        public CardVisualProperty[] properties;
        public GameObject StatsHolder;
        public GameObject ResourceHolder;

        public void LoadCard(Card c)
        {
            if (c == null)
            {
                return;
            }

            card = c;


            CloseAll();

            c.cardType.OnSetType(this);

            foreach (CardProperty prop in c.cardProperties)
            {
                CardVisualProperty cardVisualProp = GetProperty(prop.cardElement);
                if (cardVisualProp != null)
                {
                    if (prop.cardElement is CardIntegerElement)
                    {
                        cardVisualProp.text.text = prop.intValue.ToString();
                        cardVisualProp.text.gameObject.SetActive(true);
                    }
                    else if (prop.cardElement is CardTextElement)
                    {
                        cardVisualProp.text.text = prop.stringValue;
                        cardVisualProp.text.gameObject.SetActive(true);
                    }
                    else if (prop.cardElement is CardImageElement)
                    {
                        cardVisualProp.image.sprite = prop.sprite;
                        cardVisualProp.image.gameObject.SetActive(true);
                    }
                }
            }
        }

        public void CloseAll()
        {
            foreach (CardVisualProperty visualProperty in properties)
            {
                if (visualProperty.image != null)
                {
                    visualProperty.image.gameObject.SetActive(false);
                }

                if (visualProperty.text != null)
                {
                    visualProperty.text.gameObject.SetActive(false);
                }
            }
        }

        public CardVisualProperty GetProperty(CardElement e)
        {
            CardVisualProperty property = null;

            foreach (CardVisualProperty prop in properties)
            {
                if (prop.cardElement == e)
                {
                    property = prop;
                    break;
                }
            }

            return property;
        }
    }
}
