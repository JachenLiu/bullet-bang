using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Claus : SpecialCharacter
    {
        public Claus()
        {
            SetName("Claus \"The Saint\"");
            SetHealth(3);
            SetDescription("He draws one card more than the number of players, keeps 2 for himself, then gives 1 to each player.");
        }
    }
}
