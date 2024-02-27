using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Brawl : BrownCard
    {
        public Brawl(Number number, Suit suit)
        {
            SetCardName("Brawl");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
