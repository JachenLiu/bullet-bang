using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bang;

namespace Bang
{
    public class RolePile : Pile
    {
        private List<UnplayableCard> Roles = new List<UnplayableCard>();

        public List<UnplayableCard> GetPile()
        {
            return Roles;
        }

        public override void Add(Card card)
        {
            Roles.Add((UnplayableCard) card);
        }

        public override void Remove(Card card)
        {
            Roles.Remove((UnplayableCard) card);
        }
        //public void print()
        //{
        //    foreach (rolecard c in roles)
        //    {
        //        debug.log(c.getrole());
        //    }
        //}
    }
}
