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

                List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

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
