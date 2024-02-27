using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Duel : BrownCard
    {
        public Duel(Number number, Suit suit)
        {
            SetCardName("Duel");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
