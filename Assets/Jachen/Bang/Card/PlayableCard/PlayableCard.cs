using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public abstract class PlayableCard : Card
    {
        public bool IsPlayable()
        {
            return true;
        }
        public abstract void UseCard();
        public void DiscardCard()
        {
            //parameter type hand/inplay + discard pile
            //remove from hand / in play
            //add to discard pile
        }


	}

}
