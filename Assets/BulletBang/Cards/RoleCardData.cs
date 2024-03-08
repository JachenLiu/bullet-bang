using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    [CreateAssetMenu(fileName = "New Role Card", menuName = "Cards/Role Card")]
    public class RoleCardData : CardData
    {
        public RoleType role;
        public RoleCardData Initialize(RoleType role)
        {
            cardType = CardType.Role;
            cardName = role.ToString();
            this.role = role;
            return this;
        }
        public RoleType GetRole()
        {
            return role;
        }
    }
}