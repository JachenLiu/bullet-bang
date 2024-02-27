using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Panic : BrownCard
    {
        public Panic(Number number, Suit suit)
        {
            SetCardName("Panic");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}