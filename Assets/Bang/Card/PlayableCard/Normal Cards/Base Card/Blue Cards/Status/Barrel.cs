using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Barrel : BlueCard
    {
        public Barrel(Number number, Suit suit)
        {
            SetCardName("Barrel");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
