using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Holiday : DodgeCityCharacter
    {
        public Holiday()
        {
            SetName("Doc Holiday");
            SetHealth(4);
            SetDescription("During his turn, he may discard once 2 cards from the hand to shoot a BANG!.");
        }
    }
}
