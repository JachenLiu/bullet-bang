using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;
namespace Bang
{
    public class PlayerInPlay
    {
        private List<PlayableCard> InPlay = new List<PlayableCard>();

        public List<PlayableCard> getPile()
        {
            return InPlay;
        }
        public void add(PlayableCard card)
        {
            InPlay.Add(card);
        }
        public void remove(PlayableCard card)
        {
            InPlay.Remove(card);
        }
    }
}

