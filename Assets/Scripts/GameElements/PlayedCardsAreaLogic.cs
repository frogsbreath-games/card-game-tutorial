using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Areas/PlayedCardAreaWhenHoldingCard")]
    public class PlayedCardsAreaLogic : AreaLogic
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

            bool canPlay = Settings.gameManager.CurrentPlayer.CanPlayCard(card);

            if (card.cardType == creatureType)
            {

                if (canPlay)
                {
                    Settings.PlayCreatureCard(CardVariable.value.transform, areaGrid.value.transform, CardVariable.value);
                    CardVariable.value.currentLogic = playedCardLogic;
                }
                else
                {
                    Settings.RegisterEvent(Settings.gameManager.CurrentPlayer.Username + " not enough resource to play card.", Settings.gameManager.CurrentPlayer.PlayerColor);
                }

                CardVariable.value.gameObject.SetActive(true);
            }
            else if (card.cardType == ResourceType)
            {
                MultiplayerManager.Singleton.PlayerAttemptsToPlayCard(card.InstanceId, GameManager.Singleton.CurrentPlayer.PhotonId, MultiplayerManager.CardOperation.dropResourcesCard);
                //if (canPlay)
                //{
                //    //Settings.SetParentForCard(CardVariable.value.transform, ResourceGrid.value.transform);
                //    //Settings.gameManager.CurrentPlayer.AddResourceCard(CardVariable.value.gameObject);
                //    //CardVariable.value.currentLogic = playedCardLogic;
                //}
                //else
                //{
                //    Settings.RegisterEvent(Settings.gameManager.CurrentPlayer.Username + " can only play one resource card per turn.", Settings.gameManager.CurrentPlayer.PlayerColor);
                //}
                //CardVariable.value.gameObject.SetActive(true);
            } 

        }
    }
}
