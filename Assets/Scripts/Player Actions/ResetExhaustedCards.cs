using UnityEngine;
using System.Collections;

namespace PL
{
    [CreateAssetMenu(menuName ="Player Actions/Reset Exhausted Cards")]
    public class ResetExhaustedCards : PlayerAction
    {
        public override void Execute(PlayerHolder player)
        {
            MultiplayerManager.Singleton.PlayerWantsToResetFlatFootedCards(player.PhotonId);
            MultiplayerManager.Singleton.PlayerWantsToResetResourceCards(player.PhotonId);

        }
    }
}
