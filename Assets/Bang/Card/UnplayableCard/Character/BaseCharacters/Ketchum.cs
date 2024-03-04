using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Ketchum : BaseCharacter
    {
        public Ketchum()
        {
            SetName("Sid Ketchum");
            SetHealth(4);
            SetDescription("He may discard 2 cards to regain one life point.");
        }
    }
}
