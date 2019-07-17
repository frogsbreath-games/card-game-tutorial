using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public abstract class CardType : ScriptableObject
    {
        public string typeName;

        public virtual void OnSetType(CardVisual cardVisual)
        {
            CardElement typeElement = Settings.GetResourcesManager().typeElement;
            CardVisualProperty type = cardVisual.GetProperty(typeElement);
            type.text.text = typeName;
            type.text.gameObject.SetActive(true);
        }
    }
}
