using UnityEngine;
using System.Collections;

namespace PL
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public PL.GameElements.GameElementLogic currentLogic;

        public void OnClick()
        {
            if (currentLogic == null) return;
            currentLogic.OnClick(this);
        }

        public void OnHighlight()
        {
            if (currentLogic == null) return;
            Debug.Log("Highlight");
            currentLogic.OnHighlight(this);
            //Vector3 s = Vector3.one * 2;
            //this.transform.localScale = s;
        }
    }
}
