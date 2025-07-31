using Fusion;
using UnityEngine;
using System.Collections.Generic;

namespace BulletBang
{
    public class PlayerState : NetworkBehaviour
    {
        [Networked] public string PlayerName { get; set; }
        [Networked] public NetworkString<_32> PlayerId { get; set; }
        [Networked] public int MaxHealth { get; set; }
        [Networked] public int CurrentHealth { get; set; }
        [Networked] public RoleType Role { get; set; }
        [Networked] public CharacterType Character { get; set; }
        [Networked] public NetworkBool IsReady { get; set; }
        [Networked] public NetworkBool IsDead { get; set; }
        
        // References to card collections
        private List<CardData> _handCards = new List<CardData>();
        private List<CardData> _activeCards = new List<CardData>();
        
        // Distance modifiers from cards like Mustang and Scope
        [Networked] public int DistanceModifierToOthers { get; set; }
        [Networked] public int DistanceModifierFromOthers { get; set; }
        
        public override void Spawned()
        {
            if (Object.HasStateAuthority)
            {
                MaxHealth = 4; // Default health, will be modified by character
                CurrentHealth = MaxHealth;
                DistanceModifierToOthers = 0;
                DistanceModifierFromOthers = 0;
                IsDead = false;
            }
        }

        public void TakeDamage(int amount)
        {
            if (Object.HasStateAuthority)
            {
                CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
                if (CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }

        public void Heal(int amount)
        {
            if (Object.HasStateAuthority)
            {
                CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            }
        }

        private void Die()
        {
            if (Object.HasStateAuthority)
            {
                IsDead = true;
                // Trigger death effects based on role
                RPC_OnPlayerDeath();
            }
        }

        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        private void RPC_OnPlayerDeath()
        {
            // Handle death effects, like Vulture Sam's ability
            // Notify GameManager about player death
        }
    }
}