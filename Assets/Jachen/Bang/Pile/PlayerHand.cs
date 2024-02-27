using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;
namespace Bang
{
    public class PlayerHand
    {
        private List<PlayableCard> Hand = new List<PlayableCard>();

        public List<PlayableCard> getPile()
        {
            return Hand;
        }

        public void add(PlayableCard card)
        {
            Hand.Add(card);
        }
        public void remove(PlayableCard card)
        {
            Hand.Remove(card);
        }

    }
}
