using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class PlayerCard : UnplayableCard
    {
        private int ID;
        private string name;

        /*
         *private Role role 
         *private Character character
         *private int currentHealth
         *private int maxHealth
         *private Cards[] inPlay
         *private Cards[] hand
         */
        private RoleCard playerRole;
        private CharacterCard playerCharacter;
        private int maxHealth;
        private PlayerHand Hand = new PlayerHand();
        private PlayerInPlay InPlay = new PlayerInPlay();

        public PlayerCard(RoleCard role)
        {
            SetPlayerRole(role);
        }
        //get and set player name
        public string GetPlayerName()
        {
            return name;
        }

        public void SetPlayerName(string playerName)
        {
            name = playerName;
        }

        //get and set player id
        public int GetPlayerID()
        {
            return ID;
        }

        public void SetPlayerID(int playerID)
        {
            ID = playerID;
        }
        //get and set player role
        public void SetPlayerRole(RoleCard role)
        {
            playerRole = role;
        }
        public RoleCard GetPlayerRole()
        {
            return playerRole;
        }
        //get and set player character
        public void SetPlayerCharacter(CharacterCard character)
        {
            playerCharacter = character;
        }
        public CharacterCard GetPlayerCharacter()
        {
            return playerCharacter;
        }
        //get and set player max health
        public void SetPlayerMaxHealth()
        {
            maxHealth = GetPlayerCharacter().GetCharacterHealth();
            if (playerRole is Sheriff)
            {
                maxHealth += 1;
            }
        }
        public int GetPlayerMaxHealth()
        {
            return maxHealth;
        }

        //get and set playerhand
        public List<PlayableCard> GetPlayerHand()
        {
            return Hand.getPile();
        }
        public void SetPlayerHand()
        {
            Hand = new PlayerHand();
        }
        //get and set inplay

        public List<PlayableCard> GetPlayerInPlay()
        {
            return InPlay.getPile();
        }
        public void SetPlayerInPlay()
        {
            InPlay = new PlayerInPlay();
        }
    }

}