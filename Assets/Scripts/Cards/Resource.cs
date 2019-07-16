using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Cards/Resource")]
    public class Resource : CardType
    {
        public override void OnSetType(CardVisual cardVisual)
        {
            base.OnSetType(cardVisual);

            cardVisual.StatsHolder.SetActive(false);
            cardVisual.ResourceHolder.SetActive(false);
        }
    }
}
