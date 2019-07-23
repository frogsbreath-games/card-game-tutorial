using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public abstract class CardType : ScriptableObject
    {
        public string typeName;
        public bool CanAttack;

        public virtual void OnSetType(CardVisual cardVisual)
        {
            CardElement typeElement = Settings.GetResourcesManager().typeElement;
            CardVisualProperty type = cardVisual.GetProperty(typeElement);
            type.text.text = typeName;
            type.text.gameObject.SetActive(true);
        }
        
        public bool TypeCanAttack(CardInstance instance)
        {
            //Certain card type can attack even if Exhausted;
            // bool result = logic.execute if(instance.IsExhausted = false) return true;
            if (CanAttack)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
