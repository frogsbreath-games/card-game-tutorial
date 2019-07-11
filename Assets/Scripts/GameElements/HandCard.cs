using UnityEngine;
using System.Collections;
namespace PL.GameElements
{
    [CreateAssetMenu (menuName ="Game Elements/Player Hand Card")]
    public class HandCard : GameElementLogic
    {
        public SO.GameEvent onCurrentCardSelected;
        public CardVariable currentCard;
        public PL.GameStates.State heldCard;

        public override void OnClick(CardInstance instance)
        {
            currentCard.Set(instance);
            Debug.Log("Hand Card Clicked");
            Settings.gameManager.SetState(heldCard);
            onCurrentCardSelected.Raise();
        }

        public override void OnHighlight(CardInstance instance)
        {

        }
    }
}