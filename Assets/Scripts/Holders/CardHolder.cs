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
        public SO.TransformVariable AttackingLine;

        [System.NonSerialized]
        public PlayerHolder Player;

        public void LoadPlayer(PlayerHolder player, PlayerStatsVisual statVisual)
        {
            Player = player;

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

            player.Visual = statVisual;
        }

        public void SetCardOnBattleLine(CardInstance card)
        {
            Vector3 position = card.visual.gameObject.transform.position;

            Settings.SetParentForCard(card.visual.gameObject.transform, AttackingLine.value.transform);
            position.z = card.visual.gameObject.transform.position.z;
            position.y = card.visual.gameObject.transform.position.y;

            card.visual.gameObject.transform.position = position;
        }

        public void SetCardDown(CardInstance card)
        {
            Settings.SetParentForCard(card.visual.gameObject.transform, PlayedGrid.value.transform);
        }
    }
}
