using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Missed : BrownCard
    {
        public Missed(Number number, Suit suit)
        {
            SetCardName("Missed");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
