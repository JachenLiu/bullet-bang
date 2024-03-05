using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Scope : BlueCard
    {
        public Scope(Number number, Suit suit)
        {
            SetCardName("Scope");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
