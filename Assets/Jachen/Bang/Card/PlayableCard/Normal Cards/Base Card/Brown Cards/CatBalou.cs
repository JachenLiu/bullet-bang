using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class CatBalou : BrownCard
    {
        public CatBalou(Number number, Suit suit)
        {
            SetCardName("CatBalou");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
