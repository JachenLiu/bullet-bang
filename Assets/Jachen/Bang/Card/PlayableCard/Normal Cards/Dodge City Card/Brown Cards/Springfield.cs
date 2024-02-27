using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Springfield : BrownCard
    {
        public Springfield(Number number, Suit suit)
        {
            SetCardName("Springfield");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}