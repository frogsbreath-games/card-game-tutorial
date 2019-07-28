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
        public State PlayerControlState;
        public State PlayerBlockState;
        public SO.GameEvent onPlayerControlState;
        public Phase BlockPhase;

        public override void Execute(float delta)
        {
            bool mouseIsDown = Input.GetMouseButton(0);

            if (!mouseIsDown)
            {
                GameManager gameManager = Settings.gameManager;

                List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

                //Variable needs the value otherwise the comparison
                if (gameManager.Turns[gameManager.TurnIndex].currentPhase.value != BlockPhase)
                {
                    foreach (RaycastResult result in results)
                    {
                        //Check for droppable areas
                        Area a = result.gameObject.GetComponentInParent<Area>();
                        if (a != null)
                        {
                            a.OnDrop();
                            break;
                        }
                    }

                    currentCard.value.gameObject.SetActive(true);
                    currentCard.value = null;

                    Settings.gameManager.SetState(PlayerControlState);
                    onPlayerControlState.Raise();
                }
                else
                {
                    foreach (RaycastResult result in results)
                    {
                        //Check for droppable areas
                        CardInstance card = result.gameObject.GetComponentInParent<CardInstance>();

                        if (card != null)
                        {
                            int count = 0;

                            bool block = card.CanBeBlocked(currentCard.value, ref count);

                            if (block)
                            {
                                Settings.SetParentForBlock(currentCard.value.transform, card.transform, count);
                                Debug.Log("Block Card found");
                            }

                            currentCard.value.gameObject.SetActive(true);
                            currentCard.value = null;
                            onPlayerControlState.Raise();
                            Settings.gameManager.SetState(PlayerBlockState);
                            break;
                        }
                    }
                }
                return;
            }
        }
    }
}
