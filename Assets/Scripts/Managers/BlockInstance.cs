using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace PL
{
    public class BlockInstance
    {
        public List<CardInstance> Blockers = new List<CardInstance>();
        public CardInstance Attacker;
    }
}
