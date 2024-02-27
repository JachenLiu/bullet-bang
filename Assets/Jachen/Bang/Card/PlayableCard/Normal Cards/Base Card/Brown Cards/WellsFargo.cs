using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class WellsFargo : BrownCard
    {
        public WellsFargo(Number number, Suit suit)
        {
            SetCardName("WellsFargo");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
