using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PL.GameStates
{
    [CreateAssetMenu(menuName = "Actions/Select Card Click")]
    public class SelectCardClick : Action
    {
        public SO.GameEvent OnCurrentCardSelected;
        public CardVariable CurrentCard;
        public State HoldingCard; 

        public override void Execute(float delta)
        {
            if (Input.GetMouseButtonDown(0))
            {

                List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

                foreach (RaycastResult result in results)
                {
                    CardInstance instance = result.gameObject.GetComponentInParent<CardInstance>();
                    if(instance != null)
                    {
                        GameManager gameManager = Settings.gameManager;

                        if(instance.Owner != gameManager.GetOpponentOf(instance.Owner))
                        {
                            Debug.Log("Selected Card");
                            CurrentCard.value = instance;
                            gameManager.SetState(HoldingCard);
                            OnCurrentCardSelected.Raise();
                        }

                        return;
                    }
                }
            }
        }
    }
}
