using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    

    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
    public class Card : ScriptableObject
    {
        public string cardId;
        public string cardName;
        public string description;
        public CardType cardType;
        public Material frontMaterial;
        public Material backMaterial;

        public void SetMaterials(Material frontMat, Material backMat)
        {
            frontMaterial = frontMat;
            backMaterial = backMat;
        }
        protected virtual void Awake()
        {
            cardId = System.Guid.NewGuid().ToString();
        }
    }
    [CreateAssetMenu(fileName = "New Role Card", menuName = "Cards/Role Card")]
    public class RoleCard : Card
    {
        public RoleType role;
        public RoleCard Initialize(RoleType role)
        {
            cardType = CardType.Role;
            cardName = role.ToString();
            this.role = role;
            return this;
        }
    }

    // Subclass for Character cards
    [CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character Card")]
    public class CharacterCard : Card
    {
        public int characterHealth;
        public CharacterType characterName;
        public CharacterCard Initialize(CharacterType characterName)
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
    }

    public class PlayingCard : Card
    {
        public CardNumber number;
        public CardSuit suit;
        public PlayingCardType playingCardType;


        public PlayingCard Initialize(string cardName, CardNumber number, CardSuit suit)
        {
            this.cardName = cardName;
            this.number = number;
            this.suit = suit;
            cardType = CardType.Playing;
            DeterminePlayingCardType(cardName);
            return this;
        }
        private void DeterminePlayingCardType(string cardName)
        {
            switch (cardName)
            {
                case "Volcanic":
                case "Schofield":
                case "Remington":
                case "RevCarabine":
                case "Winchester":
                case "Scope":
                case "Dynamite":
                case "Mustang":
                case "Barrel":
                case "Jail":
                    playingCardType = PlayingCardType.Blue;
                    break;
                case "Panic":
                case "Beer":
                case "GeneralStore":
                case "Indians":
                case "CatBalou":
                case "Stagecoach":
                case "Gatling":
                case "WellsFargo":
                case "Duel":
                case "Saloon":
                case "Missed":
                case "Bang":
                    playingCardType = PlayingCardType.Brown;
                    break;


                default:
                    Debug.LogWarning("Unknown card name");
                    break;
            }
        }

    }
}