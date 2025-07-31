using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class DeckManager : NetworkBehaviour
    {

        [SerializeField]
        private List<RoleCardData> roleDeck = new();

        [SerializeField]
        private List<CharacterCardData> characterDeck = new();

        [SerializeField]
        private List<PlayingCardData> playingDeck = new();

        //set roles based off of max player count 8
        public void SetRolesDeck(List<RoleCardData> roleCardData, int playerCount)
        {
            foreach (RoleCardData roleCard in roleCardData)
            {
                RoleType roleType = roleCard.GetRole();
                switch (roleType)
                {
                    case RoleType.Outlaw:
                        roleDeck.Add(roleCard);
                        if (playerCount == 4 || playerCount == 5 || playerCount == 6)
                        {
                            roleDeck.Add(roleCard);
                        }
                        if ( playerCount == 7 || playerCount == 8)
                        {
                            roleDeck.Add(roleCard);
                            roleDeck.Add(roleCard);
                        }
                        continue;
                    case RoleType.Renegade:
                        roleDeck.Add(roleCard);
                        if (playerCount == 8)
                        {
                            roleDeck.Add(roleCard);
                        }
                        continue;
                    case RoleType.Deputy:
                        if (playerCount == 3 || playerCount == 5)
                        {
                            roleDeck.Add(roleCard);
                        }
                        if (playerCount == 6 || playerCount == 7 || playerCount == 8)
                        {
                            roleDeck.Add(roleCard);
                            roleDeck.Add(roleCard);
                        }
                        continue;
                    case RoleType.Sheriff:
                        if (playerCount >= 4)
                        {
                            roleDeck.Add(roleCard);
                        }
                        break;

                }
            }
        }

        public List<RoleCardData> GetRoleDeck()
        {
            foreach (RoleCardData roleCard in roleDeck)
            {
                Debug.Log(roleCard);
            }
            return roleDeck;
        }

        public void SetCharacterDeck(List<CharacterCardData> characterCardData)
        {
            //todo dodge city bool, special characters
            this.characterDeck = characterCardData;
        }
        public List<CharacterCardData> GetCharacterDeck()
        {
            return characterDeck;
        }
        public void SetPlayingDeck(List<PlayingCardData> playingCardData)
        {
            //todo dodge city
            this.playingDeck = playingCardData;
        }
        public List<PlayingCardData> GetPlayingDeck()
        {
            return playingDeck;
        }

    }
}