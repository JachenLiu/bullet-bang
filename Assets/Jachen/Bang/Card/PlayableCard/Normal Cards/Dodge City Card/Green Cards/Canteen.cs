using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Canteen : GreenCard
    {
        public Canteen(Number number, Suit suit)
        {
            SetCardName("Canteen");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
