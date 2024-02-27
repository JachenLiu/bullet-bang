using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Schofield : BlueCard
    {
        public Schofield(Number number, Suit suit)
        {
            SetCardName("Schofield");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
