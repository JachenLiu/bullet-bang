using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Sam : BaseCharacter
    {
        public Sam()
        {
            SetName("Vulture Sam");
            SetHealth(4);
            SetDescription("Whenever a player is eliminated from play, he takes in hand all the cards of that player.");
        }
    }
}
