using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Digger : DodgeCityCharacter
    {
        public Digger()
        {
            SetName("Greg Digger");
            SetHealth(4);
            SetDescription("Each time another player is eliminated, he regains 2 health.");
        }
    }
}
