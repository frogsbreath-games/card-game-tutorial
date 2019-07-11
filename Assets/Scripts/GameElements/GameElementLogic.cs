using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PL.GameElements
{
    public abstract class GameElementLogic : ScriptableObject
    {
        public abstract void OnClick(CardInstance instance);

        public abstract void OnHighlight(CardInstance instance);
    }
}
