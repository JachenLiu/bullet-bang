using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class DeckManager : MonoBehaviour
    {
        public Deck rolesDeck;
        public Deck charactersDeck;
        public Deck playingDeck;
        public void InitializeDecks(List<RoleCard> roles, List<CharacterCard> characters, List<PlayingCard> playingCards)
        {
            rolesDeck = ScriptableObject.CreateInstance<Deck>();
            rolesDeck.InitializeWithRoleCards(roles);
            rolesDeck.Shuffle();

            charactersDeck = ScriptableObject.CreateInstance<Deck>();
            charactersDeck.InitializeWithCharacterCards(characters);
            charactersDeck.Shuffle();

            playingDeck = ScriptableObject.CreateInstance<Deck>();
            playingDeck.InitializeWithPlayingCards(playingCards);
            playingDeck.Shuffle();
        }
    }
}