using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class Carabine : BlueCard
    {
        public Carabine(Number number, Suit suit)
        {
            SetCardName("Carabine");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
