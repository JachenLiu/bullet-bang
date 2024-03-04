using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class RagTime : BrownCard
    {
        public RagTime(Number number, Suit suit)
        {
            SetCardName("RagTime");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}