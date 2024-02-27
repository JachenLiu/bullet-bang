using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;

namespace Bang
{
	public abstract class RoleCard : UnplayableCard
    {
		private string roleName;
		public string GetRole()
        {
			return roleName;
        }
		public void SetRole(string role)
        {
			roleName = role;
        }
		public abstract bool IsAlive();
		public abstract bool IsSecretRole();
    }
}