﻿using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Cards/Spell")]
    public class Spell : CardType
    {
        public override void OnSetType(CardVisual cardVisual)
        {
            base.OnSetType(cardVisual);

            cardVisual.statsHolder.SetActive(false);
        }
    }
}