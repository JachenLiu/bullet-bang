using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;

namespace Bang
{
    public class DrawPile : Pile<PlayableCard>
    {
        protected override List<PlayableCard> Deck { get; set; }

        public DrawPile()
        {
            Deck = new List<PlayableCard>();

        }

        public override void Add(PlayableCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to add non playable card to draw pile");
            }
            else
            {
                Deck.Add(card);

            }
        }

        public override void Remove(PlayableCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to remove non playable card to draw pile");
            }
            else
            {
                Deck.Remove(card);
            }
        }
    }
}