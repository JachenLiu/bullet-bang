using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class DeckManager : MonoBehaviour
    {
        //decks used in game 
        public DeckInstance rolesDeck = new();
        public DeckInstance charactersDeck = new();
        public DeckInstance playingDeck = new();
       
    }
}