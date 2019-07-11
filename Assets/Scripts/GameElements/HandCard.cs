using UnityEngine;
using System.Collections;
namespace PL.GameElements
{
    [CreateAssetMenu (menuName ="Game Elements/Player Hand Card")]
    public class HandCard : GameElementLogic
    {
        public override void OnClick(CardInstance instance)
        {
            Debug.Log("Hand Card Clicked");
        }

        public override void OnHighlight(CardInstance instance)
        {

        }
    }
}