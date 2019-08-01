using UnityEngine;
using System.Collections;
namespace PL
{
    [CreateAssetMenu(menuName = "Turns/Client Empty Phase")]
    public class ClientEmptyPhase : Phase
    {
     
        public override bool IsComplete()
        {
            if (forceExit)
            {
                forceExit = false;
                return true;
            }
            return false;
        }

        public override void OnEndPhase()
        {
         
        }

        public override void OnStartPhase()
        {
         
        }
    }
}
