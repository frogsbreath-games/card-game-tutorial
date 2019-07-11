using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace PL.GameStates
{
    [CreateAssetMenu(menuName = "Actions/MouseOverDetection")]
    public class MouseOverDetection : Action
    {
        public override void Execute(float delta)
        {

            List<RaycastResult> results = Settings.GetUIObjectsUnderMouse();

            IClickable i = null;

            foreach (RaycastResult result in results)
            {
                i = result.gameObject.GetComponentInParent<IClickable>();

                if (i != null)
                {
                    i.OnHighlight();
                    break;
                }
            }
        }
    }
}
