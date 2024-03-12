using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Fusion;

namespace BulletBang
{
    public class MenuUI 
    {
        public void StartGame()
        {
            GameManager.Instance.ChangeGameState(GameState.StartGame);
        }
    }
}