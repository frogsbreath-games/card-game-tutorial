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

        private void Start()
        {
            mTransform = this.transform;
            cardVisual.gameObject.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            mTransform.position = Input.mousePosition;
        }
    }
}
