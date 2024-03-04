using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang.BaseCard;

namespace Bang.BaseCard
{
    public class BangCard : BrownCard
    {
        public BangCard(Number number, Suit suit)
        {
            SetCardName("Bang!");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
