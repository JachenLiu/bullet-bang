using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Volcanic : BlueCard
    {
        public Volcanic(Number number, Suit suit)
        {
            SetCardName("Volcanic");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
