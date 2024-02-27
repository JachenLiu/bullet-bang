using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Jack : BaseCharacter
    {
        public Jack()
        {
            SetName("Black Jack");
            SetHealth(4);
            SetDescription("He shows the second card he draws. On Heart or Diamonds, he draws one more card.");
        }
    }
}
