using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Player Actions/Draw Card From Deck")]
    public class DrawCardFromDeck : PlayerAction
    {
        public override void Execute(PlayerHolder player)
        {
            GameManager.Singleton.DrawCardFromDeck(player);
        }
    }
}