using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class IronPlate : GreenCard
    {
        public IronPlate(Number number, Suit suit)
        {
            SetCardName("IronPlate");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
