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
        private int currentHealth;
        private PlayerHand Hand;
        private PlayerInPlay InPlay;

        public PlayerCard(RoleCard role)
        {
            SetPlayerRole(role);
            SetPlayerHand();
            SetPlayerInPlay();
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
            SetPlayerMaxHealth();
            SetPlayerCurrentHealth();
        }
        public CharacterCard GetPlayerCharacter()
        {
            return playerCharacter;
        }

        public bool IsAlive()
        {
            return currentHealth > 0;
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
        public void SetPlayerCurrentHealth()
        {
            //to do
            currentHealth = maxHealth;
        }
        public int GetPlayerCurrentHealth()
        {
            return currentHealth;
        }
        public void IncreasePlayerCurrentHealth(int amount)
        {
            currentHealth += amount;
        }
        public void DecreasePlayerCurrentHealth(int amount)
        {
            currentHealth -= amount;
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

        public void AddToHand(PlayableCard card)
        {
            Hand.Add(card);
        }
        public void RemoveFromHand(PlayableCard card)
        {
            Hand.Remove(card);
        }

        public void ClearHand()
        {
            foreach(PlayableCard c in Hand)
            {
                Hand.Remove(c);
            }
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

        public void AddToInPlay(PlayableCard card)
        {
            InPlay.Add(card);
        }

        public void ClearInPlay()
        {
            foreach(PlayableCard c in InPlay)
            {
                InPlay.Remove(c);
            }
        }
        public void Print()
        {
            Debug.Log("Player iD" + GetPlayerID());
            Debug.Log("Player role" + GetPlayerRole());
            Debug.Log("Player character" + GetPlayerCharacter());
            Debug.Log("Player name" + GetPlayerName());
            Debug.Log("Player hand" + GetPlayerHand());
            Debug.Log("Player inplay" + GetPlayerInPlay());
            Debug.Log("Player maxhp" + GetPlayerMaxHealth());
            Debug.Log("Player currenthp" + GetPlayerCurrentHealth());
        }
    }

}