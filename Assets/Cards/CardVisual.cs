using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CardVisual : MonoBehaviour
{
    public Text title;
    public Text detail;
    public Text flavor;
    public Image art;

    public Card card;

    private void Start()
    {
        LoadCard(card);
    }

    public void LoadCard(Card c)
    {
        if(c == null)
        {
            return;
        }

        card = c;
        title.text = c.cardName;
        detail.text = c.cardDetail;

        if (string.IsNullOrEmpty(c.cardFlavor))
        {
            flavor.gameObject.SetActive(false);
        }
        else
        {
            flavor.gameObject.SetActive(true);
            flavor.text = c.cardFlavor;
        }

        art.sprite = c.art;

    }
}
