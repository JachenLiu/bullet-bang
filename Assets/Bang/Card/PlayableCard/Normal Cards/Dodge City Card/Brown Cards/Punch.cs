using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Punch : BrownCard
    {
        public Punch(Number number, Suit suit)
        {
            SetCardName("Punch");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}