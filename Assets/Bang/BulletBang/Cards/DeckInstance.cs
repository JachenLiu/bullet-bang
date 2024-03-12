using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class DeckInstance : MonoBehaviour
    {
        public List<CardInstance> deckCardInstances = new List<CardInstance>();

        public CardInstance cardPrefab;
        public void CreateDeckInstance(List<CardData> cardList)
        {
            foreach(CardData c in cardList)
            {
                CardInstance cardInstance = Instantiate(cardPrefab, GetRandomPosition(), Quaternion.identity, transform);
                cardInstance.SetCardData(c);
                deckCardInstances.Add(cardInstance);
            }
        }
        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-.1f, .1f), Random.Range(-.1f, .1f), 0f);
        }
        public void SetDeckData(List<CardInstance> cardInstances)
        {
            deckCardInstances= cardInstances;
        }

        public CardInstance GetCardData(int index)
        {
            return deckCardInstances[index];
        }

        public void ShuffleDeck()
        {
            int n = deckCardInstances.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                CardInstance c = deckCardInstances[k];
                deckCardInstances[k] = deckCardInstances[n];
                deckCardInstances[n] = c;
            }
        }
    }
}
/*
 * 
        public CardInstance cardPrefab;
        private CardInstance InstantiateCardData(CardData cardData, Vector3 position)
        {
            CardInstance cardInstance = Instantiate(cardPrefab, position, Quaternion.identity, transform);
            cardInstance.SetCardData(cardData);
            return cardInstance;
        }
        private Vector3 GetRandomPosition()
        {
            return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        }
        public List<CardInstance> GetRoleDeckInstance()
        {
            List<CardInstance> roles = new();
            foreach (RoleCardData roleCardData in roleDeck)
            {
                roles.Add(InstantiateCardData(roleCardData, GetRandomPosition()));
            }
            return roles;
        }


        //public void Shuffle()
        //{
        //    int n = deck.Count;
        //    while (n > 1)
        //    {
        //        n--;
        //        int k = Random.Range(0, n + 1);
        //        CardInstance c = deck[k];
        //        deck[k] = deck[n];
        //        deck[n] = c;
        //    }
        //}
        //public CardInstance GetCard(int index)
        //{
        //    return deck[index];
        //}
        //public CardInstance DrawCard()
        //{
        //    //draw card
        //    if (deck.Count > 0)
        //    {
        //        CardInstance drawnCard = deck[0];
        //        deck.RemoveAt(0);
        //        return drawnCard;
        //    }
        //    else
        //    {
        //        Debug.LogWarning("Deck is empty!");
        //        return null;
        //    }
        //}
        //public void AddCard(CardInstance card)
        //{
        //    //add card to deck
        //    deck.Add(card);
        //}
        //public void RemoveCard(CardInstance card)
        //{
        //    deck.Remove(card);
        //}
        //public void ResetDeck(List<CardInstance> deckToReset)
        //{
        //    //reset deck
        //    deck.Clear();
        //    deck.AddRange(deckToReset);
        //}
        //public bool IsEmpty()
        //{
        //    //check if deck is empty
        //    return deck.Count == 0;
        //}
        //public void AddDeck(List<CardInstance> newDeck)
        //{
        //    //combine another deck
        //    deck.AddRange(newDeck);
        //}

        //public void Print()
        //{
        //    if (deck == null)
        //    {
        //        Debug.LogWarning("Null deck");
        //    }
        //    else
        //    {
        //        Debug.Log("trying to print " + deck.ToString() + " of size " + deck.Count);
        //        foreach (CardInstance c in deck)
        //        {
        //            if (c.cardData is RoleCardData roleCard)
        //            {
        //                Debug.Log("Role is " + roleCard.GetRole());
        //            }
        //            if (c.cardData is CharacterCardData characterCard)
        //            {
        //                Debug.Log("Character is " + characterCard.GetCharacter());
        //            }
        //            if (c.cardData is PlayingCardData playingCard)
        //            {
        //                Debug.Log("Playing card is " + playingCard.GetPlayingCardType());
        //            }
        //        }
        //    }


        //}
*/