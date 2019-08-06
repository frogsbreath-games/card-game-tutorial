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

        public override void Execute()
        {
            if(CardVariable.value == null) { return; }

            Card card = CardVariable.value.visual.card;

            bool canPlay = Settings.gameManager.CurrentPlayer.CanPlayCard(card);

            if (card.cardType == creatureType)
            {
                MultiplayerManager.Singleton.PlayerAttemptsToPlayCard(card.InstanceId, GameManager.Singleton.CurrentPlayer.PhotonId, MultiplayerManager.CardOperation.dropCreatureCard);
            }
            else if (card.cardType == ResourceType)
            {
                MultiplayerManager.Singleton.PlayerAttemptsToPlayCard(card.InstanceId, GameManager.Singleton.CurrentPlayer.PhotonId, MultiplayerManager.CardOperation.dropResourcesCard);
            }
        }
    }
}
