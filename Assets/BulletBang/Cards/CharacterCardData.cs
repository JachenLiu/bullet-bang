using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{

    // Subclass for Character cards
    [CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character Card")]
    public class CharacterCardData : CardData
    {
        public int characterHealth;
        public CharacterType characterName;
        public CharacterCardData Initialize(CharacterType characterName)
        {
            cardName = characterName.ToString();
            this.characterName = characterName;
            cardType = CardType.Character;
            switch (characterName)
            {
                case CharacterType.BartCassidy:
                    characterHealth = 4;
                    description = "Each time he is hit, he draws a card.";
                    break;
                case CharacterType.BlackJack:
                    characterHealth = 4;
                    description = "He shows the second card he draws. On Heart or Diamonds, he draws one more card.";
                    break;
                case CharacterType.CalamityJanet:
                    characterHealth = 4;
                    description = "She can play BANG! cards as Missed! cards and vice versa.";
                    break;
                case CharacterType.ElGringo:
                    characterHealth = 3;
                    description = "Each time he is hit by a player, he draws a card from the hand of that player.";
                    break;
                //TODO implement other characters
                default:
                    Debug.LogWarning("Invalid character");
                    break;
            }
            return this;
        }
        public CharacterType GetCharacter()
        {
            return characterName;
        }
    }
}