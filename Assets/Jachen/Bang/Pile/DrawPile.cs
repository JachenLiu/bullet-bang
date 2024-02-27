using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;
namespace Bang
{
    public class DrawPile
    {
        private List<PlayableCard> Draw = new List<PlayableCard>();

        public List<PlayableCard> getPile()
        {
            return Draw;
        }
        public void add(PlayableCard card)
        {
            Draw.Add(card);
        }
        public void remove(PlayableCard card)
        {
            Draw.Remove(card);
        }
    }
}