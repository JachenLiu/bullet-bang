using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class CatBalou : BrownCard
    {
        public CatBalou(Number number, Suit suit)
        {
            SetCardName("Cat Balou");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}