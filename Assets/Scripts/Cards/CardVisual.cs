using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PL
{
    public class CardVisual : MonoBehaviour
    {

        public Card card;
        public CardVisualProperty[] properties;
        public GameObject statsHolder;

        private void Start()
        {
            LoadCard(card);
        }

        public void LoadCard(Card c)
        {
            if (c == null)
            {
                return;
            }

            card = c;

            c.cardType.OnSetType(this);

            foreach (CardProperty prop in c.cardProperties)
            {
                CardVisualProperty cardVisualProp = GetProperty(prop.cardElement);
                if (cardVisualProp != null)
                {
                    if (prop.cardElement is CardIntegerElement)
                    {
                        cardVisualProp.text.text = prop.intValue.ToString();
                    }
                    else if (prop.cardElement is CardTextElement)
                    {
                        cardVisualProp.text.text = prop.stringValue;
                    }
                    else if (prop.cardElement is CardImageElement)
                    {
                        cardVisualProp.image.sprite = prop.sprite;
                    }
                }
            }
            //title.text = c.cardName;
            //detail.text = c.cardDetail;

            //if (string.IsNullOrEmpty(c.cardFlavor))
            //{
            //    flavor.gameObject.SetActive(false);
            //}
            //else
            //{
            //    flavor.gameObject.SetActive(true);
            //    flavor.text = c.cardFlavor;
            //}

            //art.sprite = c.art;

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
