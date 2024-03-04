using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Beer : BrownCard
    {
        public Beer(Number number, Suit suit)
        {
            SetCardName("Beer");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
