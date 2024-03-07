using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public abstract class UnplayableCard : Card
    {
        public bool IsPlayable()
        {
            return false;
        }
    }

}