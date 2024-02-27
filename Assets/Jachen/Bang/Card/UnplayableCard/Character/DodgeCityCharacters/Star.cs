using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Star : DodgeCityCharacter
    {
        public Star()
        {
            SetName("Belle Star");
            SetHealth(4);
            SetDescription("During her turn, cards in play in front of other players have no effect.");
        }
    }
}
