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
            PointerEventData pointerData = new PointerEventData(EventSystem.current)
            {
                position = Input.mousePosition
            };

            //Raycast to determine what kind of element we are above
            List<RaycastResult> results = new List<RaycastResult>();
            //Holds a list of objects we hit 
            EventSystem.current.RaycastAll(pointerData, results);

            foreach (RaycastResult result in results)
            {
                IClickable i = result.gameObject.GetComponentInParent<IClickable>();

                if (i != null)
                {
                    //hit something clickable onclick
                    i.OnHighlight();
                }
            }
        }
    }
}
