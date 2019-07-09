using UnityEngine;
using System.Collections;

[CreateAssetMenu(menuName ="Cards/Creature")]
public class Creature : CardType
{
    public override void OnSetType(CardVisual cardVisual)
    {
        base.OnSetType(cardVisual);

        cardVisual.statsHolder.SetActive(true);
    }
}
