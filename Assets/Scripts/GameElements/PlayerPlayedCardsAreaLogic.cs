using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Areas/PlayedCardAreaWhenHoldingCard")]
    public class PlayerPlayedCardsAreaLogic : AreaLogic
    {
        public CardVariable CardVariable;
        public CardType creatureType;
        public CardType ResourceType;
        public SO.TransformVariable areaGrid;
        public SO.TransformVariable ResourceGrid;
        public PL.GameElements.GameElementLogic playedCardLogic;

        public override void Execute()
        {
            if(CardVariable.value == null) { return; }

            Card card = CardVariable.value.visual.card;

            bool canPlay = Settings.gameManager.currentPlayer.CanPlayCard(card);

            if (card.cardType == creatureType)
            {

                if (canPlay)
                {
                    Settings.PlayCreatureCard(CardVariable.value.transform, areaGrid.value.transform, card);
                    CardVariable.value.currentLogic = playedCardLogic;
                }
                
                CardVariable.value.gameObject.SetActive(true);
            }
            else if (card.cardType == ResourceType)
            {
                if (canPlay)
                {
                    Settings.SetParentForCard(CardVariable.value.transform, ResourceGrid.value.transform);
                    Settings.gameManager.currentPlayer.AddResourceCard(CardVariable.value.gameObject);
                    CardVariable.value.currentLogic = playedCardLogic;
                }
                CardVariable.value.gameObject.SetActive(true);
            } 

        }
    }
}
