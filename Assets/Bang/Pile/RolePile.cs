using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Bang
{
    public class RolePile : Pile
    {
        private List<UnplayableCard> Roles = new List<UnplayableCard>();

        public List<UnplayableCard> GetPile()
        {
            return Roles;
        }
        //public RolePile InitializeRoles(int numPlayers)
        //{

        //    Sheriff sheriff = new Sheriff();
        //    Deputy deputy = new Deputy();
        //    Renegade renegade = new Renegade();
        //    Outlaw outlaw = new Outlaw();

        //    switch (numPlayers)
        //    {
        //        case 3:
        //            Roles.Add(deputy);
        //            Roles.Add(renegade);
        //            Roles.Add(outlaw);
        //            break;
        //        case 8:
        //            Roles.Add(renegade);
        //            goto case 7;
        //        case 7:
        //            Roles.Add(deputy);
        //            goto case 6;
        //        case 6:
        //            Roles.Add(outlaw);
        //            goto case 5;
        //        case 5:
        //            Roles.Add(deputy);
        //            goto case 4;
        //        case 4:
        //            Roles.Add(sheriff);
        //            Roles.Add(renegade);
        //            Roles.Add(outlaw);
        //            Roles.Add(outlaw); // Assuming you want two outlaws in a 4-player game
        //            break;
        //    }
            
        //    return this;
        //}

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
