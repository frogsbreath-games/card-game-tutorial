using UnityEngine;
using System.Collections;

namespace PL
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public void OnClick()
        {
            throw new System.NotImplementedException();
        }

        public void OnHighlight()
        {
            Vector3 s = Vector3.one * 2;
            this.transform.localScale = s;
            Debug.Log(this.gameObject.name);
        }
    }
}
