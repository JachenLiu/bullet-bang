using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class GameManager : MonoBehaviour
    {
        private static GameManager _instance;
        public static GameManager Instance { get { return _instance;} }

        public CardManager cardManager;
        public DeckManager deckManager;
        private void Start()
        {
            InitializeGame();
        }
        private void InitializeGame()
        {
            if(cardManager == null)
            {
                Debug.LogWarning("Cardmanager not assigned");
            }
            else
            {
                cardManager.InitializeCards();
                deckManager.InitializeDecks(cardManager.GetRoleCards(), cardManager.GetCharacterCards(), cardManager.GetPlayingCards());
            }
        }

    }
}
