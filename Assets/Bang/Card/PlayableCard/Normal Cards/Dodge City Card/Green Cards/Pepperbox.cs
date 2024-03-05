using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Pepperbox : GreenCard
    {
        public Pepperbox(Number number, Suit suit)
        {
            SetCardName("Pepperbox");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
