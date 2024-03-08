using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class DeckInstance 
    {
        public List<CardInstance> deck = new List<CardInstance>();

        //public void AddRoleCards(List<RoleCard> roleCards)
        //{
        //    deck.AddRange(roleCards);
        //}
        //public void AddCharacterCards(List<CharacterCard> characterCards)
        //{
        //    deck.AddRange(characterCards);
        //}
        //public void AddPlayingCards(List<PlayingCard> playingCards)
        //{
        //    deck.AddRange(playingCards);
        //}
        //public void AddPlayerHand(List<PlayingCard> playerHand)
        //{
        //    deck.AddRange(playerHand);
        //}
        public void Shuffle()
        {
            int n = deck.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                CardInstance c = deck[k];
                deck[k] = deck[n];
                deck[n] = c;
            }
        }
        public CardInstance GetCard(int index)
        {
            return deck[index];
        }
        public CardInstance DrawCard()
        {
            //draw card
            if(deck.Count > 0)
            {
                CardInstance drawnCard = deck[0];
                deck.RemoveAt(0);
                return drawnCard;
            }
            else
            {
                Debug.LogWarning("Deck is empty!");
                return null;
            }
        }
        public void AddCard(CardInstance card)
        {
            //add card to deck
            deck.Add(card);
        }
        public void RemoveCard(CardInstance card)
        {
            deck.Remove(card);
        }
        public void ResetDeck(List<CardInstance> deckToReset)
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
        public void AddDeck(List<CardInstance> newDeck)
        {
            //combine another deck
            deck.AddRange(newDeck);
        }

        public void Print()
        {
            if (deck == null)
            {
                Debug.LogWarning("Null deck");
            }
            else
            {
                Debug.Log("trying to print " + deck.ToString() + " of size " + deck.Count);
                foreach (CardInstance c in deck)
                {
                    if (c.cardData is RoleCardData roleCard)
                    {
                        Debug.Log("Role is " + roleCard.GetRole());
                    }
                    if (c.cardData is CharacterCardData characterCard)
                    {
                        Debug.Log("Character is " + characterCard.GetCharacter());
                    }
                    if (c.cardData is PlayingCardData playingCard)
                    {
                        Debug.Log("Playing card is " + playingCard.GetPlayingCardType());
                    }
                }
            }


        }
    }
}