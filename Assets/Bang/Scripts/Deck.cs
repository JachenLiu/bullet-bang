using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    [CreateAssetMenu(fileName = "New Deck", menuName = "Cards/Deck")]
    public class Deck : ScriptableObject
    {
        public List<Card> deck = new List<Card>();

        public void InitializeWithRoleCards(List<RoleCard> roleCards)
        {
            deck.AddRange(roleCards);
        }
        public void InitializeWithCharacterCards(List<CharacterCard> characterCards)
        {
            deck.AddRange(characterCards);
        }
        public void InitializeWithPlayingCards(List<PlayingCard> playingCards)
        {
            deck.AddRange(playingCards);
        }
        public void Shuffle()
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                Card c = deck[k];
                deck[k] = deck[n];
                deck[n] = c;
            }
        }
        public Card DrawCard()
        {
            //draw card
            if(deck.Count > 0)
            {
                Card drawnCard = deck[0];
                deck.RemoveAt(0);
                return drawnCard;
            }
            else
            {
                Debug.LogWarning("Deck is empty!");
                return null;
            }
        }
        public void AddCard(Card card)
        {
            //add card to deck
            deck.Add(card);
        }
        public void ResetDeck(List<Card> deckToReset)
        {
            //reset deck
            deck.Clear();
            deck.AddRange(deckToReset);
        }
        public bool IsEmpty()
        {
            //check if deck is empty
            return deck.Count == 0;
        }
        public void AddDeck(List<Card> newDeck)
        {
            //combine another deck
            deck.AddRange(newDeck);
        }

        public void Print()
        {
            if(deck == null)
            {
                Debug.LogWarning("Deck is null");
            }
            else
            {
                foreach(Card c in deck)
                {
                    Debug.Log("Deck has card " + c); 
                }
            }
        }
    }
}