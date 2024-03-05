using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class TenGallonHat : GreenCard
    {
        public TenGallonHat(Number number, Suit suit)
        {
            SetCardName("TenGallonHat");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
