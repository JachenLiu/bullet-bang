using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;
namespace Bang
{
    public class PlayerInPlay : Pile<PlayableCard>
    {
        protected override List<PlayableCard> Deck { get; set; }

        public PlayerInPlay()
        {
            Deck = new List<PlayableCard>();
        }
        public List<PlayableCard> getPile()
        {
            return Deck;
        }

        public override void Add(PlayableCard card)
        {
            Deck.Add(card);
        }
        public override void Remove(PlayableCard card)
        {
            Deck.Remove(card);
        }
    }
}

