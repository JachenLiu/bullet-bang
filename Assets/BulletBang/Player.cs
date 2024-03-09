using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class Player : MonoBehaviour
    {
        public string playerName;
        public int playerHealth;
        public int playerMaxHealth;
        private DeckInstance playerHand;
        private DeckInstance playerInPlay;
        private RoleCardData playerRole;
        private CharacterCardData playerCharacter;

        public void SetPlayerName(string playerName)
        {
            this.playerName = playerName;
        }
        public string GetPlayerName()
        {
            return playerName;
        }
        public void SetPlayerHealth(int playerHealth)
        {
            this.playerHealth = playerHealth;
        }
        public int GetPlayerHealth()
        {
            return playerHealth;
        }
        public void SetPlayerMaxHealth(int playerMaxHealth)
        {
            this.playerMaxHealth = playerMaxHealth;
        }
        public int GetPlayerMaxHealth()
        {
            return playerMaxHealth;
        }
        public void SetPlayerHand(DeckInstance playerHand)
        {
            this.playerHand = playerHand;
        }
        public DeckInstance GetPlayerHand()
        {
            return playerHand;
        }
        public void SetPlayerInPlay(DeckInstance playerInPlay)
        {
            this.playerInPlay = playerInPlay;
        }
        public DeckInstance GetPlayerInPlay()
        {
            return playerInPlay;
        }
        public void SetPlayerRole(RoleCardData playerRole)
        {
            this.playerRole = playerRole;
        }
        public RoleCardData GetPlayerRole()
        {
            return playerRole;
        }
        public void SetPlayerCharacter(CharacterCardData playerCharacter)
        {
            this.playerCharacter = playerCharacter;
        }
        public CharacterCardData GetPlayerCharacter()
        {
            return playerCharacter;
        }

        public bool HasRole()
        {
            return playerRole != null;
        }
        public bool HasCharacter()
        {
            return playerCharacter != null;
        }
        public bool IsAlive()
        {
            return playerHealth > 0;
        }
        public void TakesDamage(int damage)
        {
            playerHealth = playerHealth - damage;
        }
        public void HealsDamage(int heal)
        {
            playerHealth = playerHealth + heal;
        }
        //public CardInstance UseCardFromHand(CardInstance usedCard)
        //{
        //    playerHand.RemoveCard(usedCard);
        //    return usedCard;
        //}
        //public CardInstance UseCardFromInPlay(CardInstance usedCard)
        //{
        //    playerInPlay.RemoveCard(usedCard);
        //    return usedCard;
        //}
        //public void AddCardToHand(CardInstance drawnCard)
        //{
        //    playerHand.AddCard(drawnCard);
        //}
        //public void UseCardFromHandToInPlay(CardInstance usedCard)
        //{
        //    playerHand.RemoveCard(usedCard);
        //    playerInPlay.AddCard(usedCard);
        //}

    }
}