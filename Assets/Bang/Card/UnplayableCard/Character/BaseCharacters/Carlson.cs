using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Carlson : BaseCharacter
    {
        public Carlson()
        {
            SetName("Kit Carlson");
            SetHealth(4);
            SetDescription("He looks at the top 3 cards of the deck and chooses the 2 to draw.");
        }
    }
}
