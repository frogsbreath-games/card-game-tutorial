﻿using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Areas/PlayedCardAreaWhenHoldingCard")]
    public class PlayerPlayedCardsAreaLogic : AreaLogic
    {
        public CardVariable card;
        public CardType creatureType;
        public SO.TransformVariable areaGrid;
        public PL.GameElements.GameElementLogic playedCardLogic;

        public override void Execute()
        {
            if(card.value == null) { return; }

            if(card.value.visual.card.cardType == creatureType)
            {

                Debug.Log("Place Card Down");
                Settings.SetParentForCard(card.value.transform, areaGrid.value.transform);
                card.value.currentLogic = playedCardLogic;
                card.value.gameObject.SetActive(true);
                //Place card down
            }

        }
    }
}