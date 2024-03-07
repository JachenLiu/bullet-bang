using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class CanCan : GreenCard
    {
        public CanCan(Number number, Suit suit)
        {
            SetCardName("CanCan");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
