using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PL.GameStates
{
    [CreateAssetMenu(menuName = "Actions/OnMouseClick")]
    public class OnMouseClick : Action
    {
        public override void Execute(float delta)
        {
            if (Input.GetMouseButtonDown(0))
            {
                PointerEventData pointerData = new PointerEventData(EventSystem.current)
                {
                    position = Input.mousePosition
                };

                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(pointerData, results);

                foreach (RaycastResult result in results)
                {
                    IClickable i = result.gameObject.GetComponentInParent<IClickable>();
                    if (i != null)
                    {
                        i.OnClick();
                        break;
                    }
                    else
                    {
                        Debug.Log(result.gameObject.name);
                    }
                }
            }
        }
    }
}
