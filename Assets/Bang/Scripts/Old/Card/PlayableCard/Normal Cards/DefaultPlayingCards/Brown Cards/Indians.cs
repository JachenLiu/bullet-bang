using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Indians : BrownCard
    {
        public Indians(Number number, Suit suit)
        {
            SetCardName("Indians");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
