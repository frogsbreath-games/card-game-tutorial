using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName ="Player Actions/Reset Exhausted Cards")]
    public class ResetExhaustedCards : PlayerAction
    {
        public override void Execute(PlayerHolder player)
        {
            foreach (CardInstance instance in player.PlayedCards)
            {
                if (instance.IsExhausted)
                {
                    instance.visual.transform.localEulerAngles = Vector3.zero;
                    instance.IsExhausted = false;
                }
            }
        }
    }
}
