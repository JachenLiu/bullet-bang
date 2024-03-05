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
        public void Print()
        {
            if (Deck == null)
            {
                Debug.LogWarning("Null deck");
            }
            else
            {
                Debug.LogWarning("trying to print");
                Debug.LogWarning(Deck.ToString()+ Deck.Count);
                foreach (T c in Deck)
                {
                    if (c is RoleCard roleCard)
                    {
                        Debug.LogWarning("Role" + roleCard.GetRole());
                    }
                    if (c is CharacterCard characterCard)
                    {
                        Debug.LogWarning("character" + characterCard.GetName());
                    }
                    if (c is PlayerCard playerCard)
                    {
                        Debug.LogWarning("player " + playerCard.GetPlayerRole());
                    }
                    if (c is PlayableCard playableCard)
                    {
                        Debug.LogWarning("playable card is " + playableCard.ToString());
                    }
                }
            }
            

        }
    }
}