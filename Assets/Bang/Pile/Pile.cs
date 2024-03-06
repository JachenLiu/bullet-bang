using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{

    public abstract class Pile<T> where T : Card
    {
        protected abstract List<T> Deck { get; set; }
        public abstract void Add(T card);
        public abstract void Remove(T card);
        public List<T> AddRange(Pile<T> pile)
        {
            Deck.AddRange(pile.Deck);
            return Deck;
        }
        public T GetCard(int index)
        {
            return Deck[index];
        }
        public void SetCard(T card, int index)
        {
            Deck[index] = card;
        }

        public void Shuffle()
        {
            int n = Deck.Count;
            while (n > 1)
            {
                n--;
                int k = Random.Range(0, n + 1);
                T c = Deck[k];
                Deck[k] = Deck[n];
                Deck[n] = c;
            }
        }
        public T Draw()
        {
            if(Deck.Count == 0)
            {
                Debug.LogError("Deck is empty");
                return default(T);
            }
            T drawnCard = Deck[0];
            Deck.RemoveAt(0);

            return drawnCard;
        }
        public IEnumerator<T> GetEnumerator()
        {
            return Deck.GetEnumerator();
        }
        public void Print()
        {
            if (Deck == null)
            {
                Debug.LogWarning("Null deck");
            }
            else
            {
                Debug.LogWarning("trying to print " + Deck.ToString() + " of size " + Deck.Count);
                foreach (T c in Deck)
                {
                    if (c is RoleCard roleCard)
                    {
                        Debug.LogWarning("Role is" + roleCard.GetRole());
                    }
                    if (c is CharacterCard characterCard)
                    {
                        Debug.LogWarning("Character is " + characterCard.GetName());
                    }
                    if (c is PlayerCard playerCard)
                    {
                        Debug.LogWarning("Player is a" + playerCard.GetPlayerRole());
                    }
                    if (c is NormalCard normalCard)
                    {
                        Debug.LogWarning("Normal card is " + normalCard.ToString());
                    }
                }
            }
            

        }
    }
}