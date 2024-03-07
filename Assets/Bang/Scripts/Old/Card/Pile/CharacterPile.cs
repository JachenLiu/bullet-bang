using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;

namespace Bang
{
    public class CharacterPile : Pile<CharacterCard>
    {
        protected override List<CharacterCard> Deck { get; set; }

        public CharacterPile()
        {
            Deck = new List<CharacterCard>();

        }

        public override void Add(CharacterCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to add non character card to character pile");
            }
            else
            {
                Deck.Add(card);

            }
        }

        public override void Remove(CharacterCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to remove non character card to character pile");
            }
            else
            {
                Deck.Remove(card);
            }
        }
    }
}