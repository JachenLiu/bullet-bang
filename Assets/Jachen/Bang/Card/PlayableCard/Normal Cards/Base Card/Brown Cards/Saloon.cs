using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Saloon : BrownCard
    {
        public Saloon(Number number, Suit suit)
        {
            SetCardName("Saloon");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
