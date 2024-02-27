using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class ClientEvent : GameEvent
    {
        //creating lobby
        public void PlayerCreatesLobby()
        {

        }
        public void PlayerJoinsLobby()
        {

        }
        public void HostSelects()
        {
            //Host selects cards to be used
        }
        //starts game
        public void HostStartsGame()
        {

        }
        //in game events
        public void PlayerConfirmsRole()
        {
            //Role is set to player
        }
        public void PlayerChoosesCharacter()
        {
            //Character is set to player
        }
        public void PlayerDrawsCard()
        {
            //Card is added from DrawPile, Discard, or (Another) PlayerHand to PlayerInHandPile
            //
        }
        public void PlayerUsesCard()
        {
            //Card is added to the DiscardPile or PlayerInPlayPile from PlayerHand
        }
        public void PlayerDiscardsCard()
        {
            //Card is added to the DiscardPile from PlayerHand or PlayerInPlay
        }
        public void PlayerReacts()
        {
            //Player does action if targeted by another player
        }
        public void PlayerQuitsGame()
        {
            //Player leaves lobby
        }
        public void PlayerSpectatesGame()
        {
            //Player spectates game
        }
    }
}

