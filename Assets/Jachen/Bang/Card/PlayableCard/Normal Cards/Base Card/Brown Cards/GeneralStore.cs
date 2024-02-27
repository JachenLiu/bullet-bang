using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class GeneralStore : BrownCard
    {
        public GeneralStore(Number number, Suit suit)
        {
            SetCardName("GeneralStore");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
