using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName ="Holders/Card Holder")]
    public class CardHolder : ScriptableObject
    {
        public SO.TransformVariable HandGrid;
        public SO.TransformVariable ResourceGrid;
        public SO.TransformVariable PlayedGrid;

        public void LoadPlayer(PlayerHolder player)
        {
            foreach (CardInstance card in player.HandCards)
            {
                Settings.SetParentForCard(card.visual.gameObject.transform, HandGrid.value.transform);
            }

            foreach (CardInstance card in player.PlayedCards)
            {
                Settings.SetParentForCard(card.visual.gameObject.transform, PlayedGrid.value.transform);
            }

            foreach (ResourceHolder resourceCard in player.ResourceHolderList)
            {
                Settings.SetParentForCard(resourceCard.ResourceCard.transform, ResourceGrid.value.transform);
            }
        }
    }
}
