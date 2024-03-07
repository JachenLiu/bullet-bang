using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Wengam : DodgeCityCharacter
    {
        public Wengam()
        {
            SetName("Chuck Wengam");
            SetHealth(4);
            SetDescription("During his turn, he may choose to lose 1 health to draw 2 cards.");
        }
    }
}
