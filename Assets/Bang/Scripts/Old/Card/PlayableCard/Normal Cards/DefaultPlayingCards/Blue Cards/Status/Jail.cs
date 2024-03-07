using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Jail : BlueCard
    {
        public Jail(Number number, Suit suit)
        {
            SetCardName("Jail");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
