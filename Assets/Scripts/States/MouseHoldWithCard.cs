using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using PL.GameElements;

namespace PL.GameStates
{
    [CreateAssetMenu(menuName = "Actions/MouseHoldWithCard")]
    public class MouseHoldWithCard : Action
    {
        public CardVariable currentCard;
        public State playerControlState;
        public SO.GameEvent onPlayerControlState;

        public override void Execute(float delta)
        {
            bool mouseIsDown = Input.GetMouseButton(0);

            if (!mouseIsDown)
            {
                List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

                foreach (RaycastResult result in results)
                {
                    //Check for droppable areas
                    Area a = result.gameObject.GetComponentInParent<Area>();
                    if(a != null)
                    {
                        a.OnDrop();
                        break;
                    }
                }

                currentCard.value.gameObject.SetActive(true);
                currentCard.value = null;

                Settings.gameManager.SetState(playerControlState);
                onPlayerControlState.Raise();
                return;
            }
        }
    }
}
