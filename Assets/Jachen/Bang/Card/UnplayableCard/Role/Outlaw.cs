using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public class Outlaw : RoleCard
    {
        public Outlaw()
        {
            SetRole("Outlaw");
        }

        public override bool IsAlive()
        {
            return true;
        }
        public override bool IsSecretRole()
        {
            return true;
        }
    }
}