using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Player
    {
        private int ID;
        private string name;

        /*
         *private Role role 
         *private Character character
         *private int currentHealth
         *private int maxHealth
         *private Cards[] inPlay
         *private Cards[] hand
         */

        PlayerHand Hand = new PlayerHand();
        PlayerInPlay InPlay = new PlayerInPlay();

        public string GetPlayerName()
        {
            return name;
        }

        public void SetPlayerName(string playerName)
        {
            name = playerName;
        }

        public int GetPlayerID()
        {
            return ID;
        }

        public void SetPlayerID(int playerID)
        {
            ID = playerID;
        }
        public List<PlayableCard> GetPlayerHand()
        {
            return Hand.getPile();
        }

        public List<PlayableCard> GetPlayerInPlay()
        {
            return InPlay.getPile();
        }
    }
}