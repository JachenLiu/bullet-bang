using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public abstract class Pile<T>
    {
        protected abstract List<T> Deck { get; set; }
    }
    public abstract class Pile : Pile<Card>
    {
        protected override List<Card> Deck { get; set; }

        public abstract void Add(Card card);
        public abstract void Remove(Card card);
        public List<Card> Append(Pile pile)
        {
            //todo
            return pile.Deck;
        }
        public void Shuffle()
        {
            int n = Deck.Count;
            while(n > 1)
            {
                n--;
                int k = Random.Range(1, n);
                Card c = Deck[k];
                Deck[k] = Deck[n];
                Deck[n] = c;
            }
        }
        public void Print()
        {
            foreach (Card c in Deck)
            {
                Debug.Log(c);
            }
        }
    }
}