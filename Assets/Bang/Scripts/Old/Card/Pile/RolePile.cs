using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class RolePile : Pile<RoleCard>
    {
        protected override List<RoleCard> Deck { get; set; }

        public RolePile()
        {
            Deck = new List<RoleCard>();
        }

        public override void Add(RoleCard card)
        {
            if(card == null)
            {
                Debug.LogError("tried to add non role card to role pile");
            }
            else
            {
                Deck.Add(card);

            }
        }

        public override void Remove(RoleCard card)
        {
            if (card == null)
            {
                Debug.LogError("tried to remove non role card to role pile");
            }
            else
            {
                Deck.Remove(card);
            }
        }
    }
}
