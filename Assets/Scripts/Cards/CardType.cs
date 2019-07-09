using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardType : ScriptableObject
{
    public string typeName;

    public virtual void OnSetType(CardVisual cardVisual)
    {
        CardElement typeElement = GameManager.GetResourcesManager().typeElement;
        CardVisualProperty type = cardVisual.GetProperty(typeElement);
        type.text.text = typeName;
    }
}
