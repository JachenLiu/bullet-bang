using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Remington : BlueCard
    {
        public Remington(Number number, Suit suit)
        {
            SetCardName("Remington");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
