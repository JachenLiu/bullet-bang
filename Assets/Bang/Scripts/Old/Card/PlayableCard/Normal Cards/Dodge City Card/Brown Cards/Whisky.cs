using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Whisky : BrownCard
    {
        public Whisky(Number number, Suit suit)
        {
            SetCardName("Whisky");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}