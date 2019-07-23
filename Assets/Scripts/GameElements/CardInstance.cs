using UnityEngine;
using System.Collections;

namespace PL
{
    public class CardInstance : MonoBehaviour, IClickable
    {
        public PL.GameElements.GameElementLogic currentLogic;
        public CardVisual visual;
        public bool IsExhausted;

        public void Start()
        {
            visual = GetComponent<CardVisual>();
        }

        public void OnClick()
        {
            if (currentLogic == null) return;
            currentLogic.OnClick(this);
        }

        public void OnHighlight()
        {
            if (currentLogic == null) return;
            currentLogic.OnHighlight(this);
            //Vector3 s = Vector3.one * 2;
            //this.transform.localScale = s;
        }

        public bool CanAttack()
        {
            bool result = true;

            if (IsExhausted)
            {
                result = false;
            }

            if (visual.card.cardType.TypeCanAttack(this))
            {
                result = true;
            }

            return result;
        }
    }
}
