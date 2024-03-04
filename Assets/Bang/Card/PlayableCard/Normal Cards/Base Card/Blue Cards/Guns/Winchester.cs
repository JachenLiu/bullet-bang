using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Winchester : BlueCard
    {
        public Winchester(Number number, Suit suit)
        {
            SetCardName("Winchester");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
