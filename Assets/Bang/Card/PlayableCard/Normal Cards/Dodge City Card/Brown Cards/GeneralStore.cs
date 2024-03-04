using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class GeneralStore : BrownCard
    {
        public GeneralStore(Number number, Suit suit)
        {
            SetCardName("General Store");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}