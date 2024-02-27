using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Knife : GreenCard
    {
        public Knife(Number number, Suit suit)
        {
            SetCardName("Knife");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
