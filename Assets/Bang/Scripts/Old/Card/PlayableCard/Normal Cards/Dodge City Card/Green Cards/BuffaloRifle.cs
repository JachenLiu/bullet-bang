using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class BuffaloRifle : GreenCard
    {
        public BuffaloRifle(Number number, Suit suit)
        {
            SetCardName("BuffaloRifle");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
