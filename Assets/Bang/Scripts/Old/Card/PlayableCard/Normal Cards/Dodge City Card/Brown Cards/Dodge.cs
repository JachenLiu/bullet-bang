using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Dodge : BrownCard
    {
        public Dodge(Number number, Suit suit)
        {
            SetCardName("Dodge");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}