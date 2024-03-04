using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Mustang : BlueCard
    {
        public Mustang(Number number, Suit suit)
        {
            SetCardName("Mustang");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
