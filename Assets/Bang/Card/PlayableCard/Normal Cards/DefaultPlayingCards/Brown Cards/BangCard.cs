using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class BangCard : BrownCard
    {
        public BangCard(Number number, Suit suit)
        {
            SetCardName("Bang!");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
