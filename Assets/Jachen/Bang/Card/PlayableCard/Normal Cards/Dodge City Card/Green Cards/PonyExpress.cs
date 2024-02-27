using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class PonyExpress : GreenCard
    {
        public PonyExpress(Number number, Suit suit)
        {
            SetCardName("PonyExpress");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
