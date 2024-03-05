using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Conestoga : GreenCard
    {
        public Conestoga(Number number, Suit suit)
        {
            SetCardName("Conestoga");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
