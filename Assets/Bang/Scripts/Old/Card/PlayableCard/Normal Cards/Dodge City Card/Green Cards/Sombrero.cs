using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Sombrero : GreenCard
    {
        public Sombrero(Number number, Suit suit)
        {
            SetCardName("Sombrero");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
