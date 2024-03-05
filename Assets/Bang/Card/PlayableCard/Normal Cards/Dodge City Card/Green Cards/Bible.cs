using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Bible : GreenCard
    {
        public Bible(Number number, Suit suit)
        {
            SetCardName("Bible");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
