using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class PlayerPile : Pile<PlayerCard>
    {
        protected override List<PlayerCard> Deck { get; set; }

        public PlayerPile()
        {
            Deck = new List<PlayerCard>();
        }

        public override void Add(PlayerCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to add non player card to player pile");
            }
            else
            {
                Deck.Add(card);

            }
        }

        public override void Remove(PlayerCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to remove non player card to player pile");
            }
            else
            {
                Deck.Remove(card);
            }
        }
    }
}
