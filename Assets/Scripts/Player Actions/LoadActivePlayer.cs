using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName = "Player Actions/Load Active Player")]
    public class LoadActivePlayer : PlayerAction
    {
        public override void Execute(PlayerHolder player)
        {
            GameManager.Singleton.LoadPlayerActive(player);
        }
    }
}
