using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang.DodgeCity
{
    public class Binocular : BlueCard
    {
        public Binocular(Number number, Suit suit)
        {
            SetCardName("Binocular");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
