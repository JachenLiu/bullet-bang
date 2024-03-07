using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Howitzer : GreenCard
    {
        public Howitzer(Number number, Suit suit)
        {
            SetCardName("Howitzer");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
