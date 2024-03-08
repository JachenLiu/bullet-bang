using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    [CreateAssetMenu(fileName = "New Role Card", menuName = "Cards/Role Card")]
    public class RoleCardData : CardData
    {
        [SerializeField]
        private RoleType role;
        public RoleType GetRole()
        {
            return role;
        }
    }
}