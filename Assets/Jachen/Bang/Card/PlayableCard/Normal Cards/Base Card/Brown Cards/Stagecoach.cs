using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang.BaseCard
{
    public class Stagecoach : BrownCard
    {
        public Stagecoach(Number number, Suit suit)
        {
            SetCardName("Stagecoach");
            SetCardNumber(number);
            SetCardSuit(suit);
        }
    }
}
