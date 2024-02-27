using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class GameState
    {
        public GameState m_GameState { get; private set; }

        public void SetGameState(GameState gameState)
        {
            m_GameState = gameState;
        }
    }
}

