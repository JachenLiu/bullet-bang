using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class ServerEvent : GameEvent
    {
        private List<Player> playerLobby;
        private RolePile rolePile;
        public void CreateLobby()
        {
            //Lobby is created by host player
            playerLobby = new();
            //todo generate lobby code
        }
        public void JoinLobby(/**/)
        {
            //Player is added to PlayerLobby using lobby code

        }
        public void DealRoles(int numPlayers)
        {
            //Give each player a Role card
            //rolePile = new();
            //switch (numPlayers)
            //{
            //    case 3:
            //        rolePile.add(DEPUTY);
            //        rolePile.add(RENEGADE);
            //        rolePile.add(OUTLAW);
            //        break;

            //    case 8:
            //        rolePile.add(RENEGADE);
            //        goto case 7;

            //    case 7:
            //        rolePile.add(DEPUTY);
            //        goto case 6;

            //    case 6:
            //        rolePile.add(OUTLAW);
            //        goto case 5;

            //    case 5:
            //        rolePile.add(DEPUTY);
            //        goto case 4;

            //    case 4:
            //        rolePile.add(SHERIFF);
            //        rolePile.add(RENEGADE);
            //        rolePile.add(OUTLAW);
            //        rolePile.add(OUTLAW);
            //        break;

            //}
            //foreach (Player p in playerLobby)
            //{
            //    RolePile.shuffle
            //    p.GetPlayerHand().Add(/*random card from rolepile*/)
            //}
        }
        public void SetRole()
        {
            //Set role to player
        }
        public void DealCharacters()
        {
            //Give each player 2 Character cards
        }
        public void SetCharacter()
        {
            //Set character to player
        }
        public void BeginGame()
        {
            //Set turn to Sheriff player
        }
        public void EndGame()
        {
            //reset game state
        }
        public void UseBang()
        {
            //
        }

    }
}

