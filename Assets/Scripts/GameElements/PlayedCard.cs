using UnityEngine;
using System.Collections;
namespace PL.GameElements
{
    [CreateAssetMenu(menuName = "Game Elements/Player Played Card")]
    public class PlayedCard : GameElementLogic
    {
        public override void OnClick(CardInstance instance)
        {
            Debug.Log("Played Card Clicked");
        }

        public override void OnHighlight(CardInstance instance)
        {

        }
    }
}