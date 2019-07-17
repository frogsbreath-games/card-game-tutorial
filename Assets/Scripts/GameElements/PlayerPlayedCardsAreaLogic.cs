using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Areas/PlayedCardAreaWhenHoldingCard")]
    public class PlayerPlayedCardsAreaLogic : AreaLogic
    {
        public CardVariable card;
        public CardType creatureType;
        public CardType ResourceType;
        public SO.TransformVariable areaGrid;
        public SO.TransformVariable ResourceGrid;
        public PL.GameElements.GameElementLogic playedCardLogic;

        public override void Execute()
        {
            if(card.value == null) { return; }

            if(card.value.visual.card.cardType == creatureType)
            {

                Settings.SetParentForCard(card.value.transform, areaGrid.value.transform);
                card.value.currentLogic = playedCardLogic;
                card.value.gameObject.SetActive(true);
            }
            else if (card.value.visual.card.cardType == ResourceType)
            {
                Settings.SetParentForCard(card.value.transform, ResourceGrid.value.transform);
                card.value.currentLogic = playedCardLogic;
                card.value.gameObject.SetActive(true);
            }

        }
    }
}
