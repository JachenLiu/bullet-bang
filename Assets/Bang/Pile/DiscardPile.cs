using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;
namespace Bang
{
    public class DiscardPile
    {
        private List<PlayableCard> Discard = new List<PlayableCard>();

        public List<PlayableCard> getPile()
        {
            return Discard;
        }
        public void add(PlayableCard card)
        {
            Discard.Add(card);
        }
        public void remove(PlayableCard card)
        {
            Discard.Remove(card);
        }
    }
}