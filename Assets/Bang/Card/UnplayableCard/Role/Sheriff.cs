using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
    public class Sheriff : RoleCard
    {
        public Sheriff()
        {
            SetRole("Sheriff");
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