using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Dynamite : BlueCard
    {
        public Dynamite(Number number, Suit suit)
        {
            SetCardName("Dynamite");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
