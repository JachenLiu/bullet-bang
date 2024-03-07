using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class CardManager : MonoBehaviour
    {
        private List<RoleCard> roleCards;
        private List<CharacterCard> characterCards;
        private List<PlayingCard> playingCards;
        public void InitializeCards()
        {
            //Initialize role cards
            roleCards = new List<RoleCard>();
            foreach (RoleType roleType in System.Enum.GetValues(typeof(RoleType)))
            {
                RoleCard roleCard = ScriptableObject.CreateInstance<RoleCard>();
                roleCard.Initialize(roleType);
                roleCards.Add(roleCard);
                Debug.Log("Added " + roleCard);
            }

            //Initialize character cards
            characterCards = new List<CharacterCard>();
            foreach (CharacterType characterType in System.Enum.GetValues(typeof(CharacterType)))
            {
                CharacterCard characterCard = ScriptableObject.CreateInstance<CharacterCard>();
                characterCard.Initialize(characterType);
                characterCards.Add(characterCard);
            }

            //Initialize playing cards
            playingCards = new List<PlayingCard>();
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Volcanic", CardNumber.Ten , CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Volcanic", CardNumber.Ten, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Schofield", CardNumber.Jack, CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Schofield", CardNumber.King, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Schofield", CardNumber.Queen, CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Remington", CardNumber.King, CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("RevCarabine", CardNumber.Ace, CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Winchester", CardNumber.Eight, CardSuit.Club));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Scope", CardNumber.Ace, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Dynamite", CardNumber.Two, CardSuit.Heart));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Mustang", CardNumber.Eight, CardSuit.Heart));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Mustang", CardNumber.Nine, CardSuit.Heart));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Barrel", CardNumber.King, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Barrel", CardNumber.Queen, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Jail", CardNumber.Jack, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Jail", CardNumber.Queen, CardSuit.Spade));
            playingCards.Add(ScriptableObject.CreateInstance<PlayingCard>().Initialize("Jail", CardNumber.Ten, CardSuit.Spade));


        }
        public List<RoleCard> GetRoleCards()
        {
            return roleCards;
        }
        public List<CharacterCard> GetCharacterCards()
        {
            return characterCards;
        }
        public List<PlayingCard> GetPlayingCards()
        {
            return playingCards;
        }

    }

}