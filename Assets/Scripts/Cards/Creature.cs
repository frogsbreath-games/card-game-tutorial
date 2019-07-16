using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Cards/Creature")]
    public class Creature : CardType
    {
        public override void OnSetType(CardVisual cardVisual)
        {
            base.OnSetType(cardVisual);

            cardVisual.StatsHolder.SetActive(true);
        }
    }
}
