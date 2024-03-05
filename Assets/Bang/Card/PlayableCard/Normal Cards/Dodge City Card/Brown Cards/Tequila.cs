using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Tequila : BrownCard
    {
        public Tequila(Number number, Suit suit)
        {
            SetCardName("Tequila");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}