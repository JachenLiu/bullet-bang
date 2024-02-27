using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Derringer : GreenCard
    {
        public Derringer(Number number, Suit suit)
        {
            SetCardName("Derringer");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
