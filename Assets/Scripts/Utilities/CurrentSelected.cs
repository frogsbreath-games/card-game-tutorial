using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL
{
    public class CurrentSelected : MonoBehaviour
    {
        public CardVariable currentCard;
        public CardVisual cardVisual;

        public Transform mTransform;

        public void LoadCard()
        {
            if (currentCard == null) { return; }
            currentCard.value.gameObject.SetActive(false);
            cardVisual.LoadCard(currentCard.value.visual.card);
            cardVisual.gameObject.SetActive(true);
        }

        public void CloseCard()
        {
            cardVisual.gameObject.SetActive(false);
        }

        private void Start()
        {
            CloseCard();
            mTransform = this.transform;
        }

        // Update is called once per frame
        void Update()
        {
            mTransform.position = Input.mousePosition;
        }
    }
}
