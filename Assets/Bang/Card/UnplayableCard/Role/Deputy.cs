using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public class Deputy : RoleCard
    {
        public Deputy()
        {
            //create sheriff role
            SetRole("Deputy");
        }

        public override bool IsAlive()
        {
            return true;
        }
        public override bool IsSecretRole()
        {
            return false;
        }
    }
}