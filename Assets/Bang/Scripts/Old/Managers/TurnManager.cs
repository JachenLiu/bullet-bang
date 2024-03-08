using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class TurnManager
    {
        private PlayerPile players;
        //private int currentPlayerIndex;
        public TurnManager(PlayerPile players)
        {
            this.players = players;
            //this.currentPlayerIndex = 0; //first player is current - change to sheriff
        }

        public void EndTurn()
        {

        }
        public void StartNextTurn()
        {

        }
    }
}