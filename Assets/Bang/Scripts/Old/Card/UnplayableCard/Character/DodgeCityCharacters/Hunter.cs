using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Hunter : DodgeCityCharacter
    {
        public Hunter()
        {
            SetName("Herb Hunter");
            SetHealth(4);
            SetDescription("Each time another player is eliminated, he draws 2 extra cards.");
        }
    }
}
