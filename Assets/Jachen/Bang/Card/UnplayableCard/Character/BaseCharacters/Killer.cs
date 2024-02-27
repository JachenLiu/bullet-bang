using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Killer : BaseCharacter
    {
        public Killer()
        {
            SetName("Slab the Killer");
            SetHealth(4);
            SetDescription("Player needs 2 Missed! cards to cancel his BANG! card.");
        }
    }
}
