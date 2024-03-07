using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JachenGame.GameSystem
{
    public class FakeGameManager
    {
        /* 
         * Lots of game manager code
         */


        public void StartGame()
        {
            Debug.Log("Started Game yaya");
        }
        public void JoinSession(Player playerThatWantsToPlay)
        {

        }


    }


    public class Player
    {
        public string ID;
    }
}