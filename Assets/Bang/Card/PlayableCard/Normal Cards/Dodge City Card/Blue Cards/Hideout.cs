using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Hideout : BlueCard
    {
        public Hideout(Number number, Suit suit)
        {
            SetCardName("Hideout");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
