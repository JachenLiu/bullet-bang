using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public class Renegade : RoleCard
    {
        public Renegade()
        {
            SetRole("Renegade");
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