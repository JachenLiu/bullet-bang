using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Gringo : BaseCharacter
    {
        public Gringo()
        {
            SetName("El Gringo");
            SetHealth(3);
            SetDescription("Each time he is hit by a player, he draws a card from the hand of that player.");
        }
    }
}
