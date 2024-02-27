using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Gatling : BrownCard
    {
        public Gatling(Number number, Suit suit)
        {
            SetCardName("Gatling");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
