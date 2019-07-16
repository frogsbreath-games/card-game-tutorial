using UnityEngine;
using System.Collections;
using SO.UI;
using SO;
using UnityEngine.UI;

namespace PL { 
    public class UpdateTextFromPhase : UIPropertyUpdater
    {
        public PhaseVariable targetPhase;
        public Text targetText;

        /// <summary>
        /// Use this to update a text UI element based on the target string variable
        /// </summary>
        public override void Raise()
        {
            targetText.text = targetPhase.value.phaseName;
        }
    }
}