using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class TurnManager : MonoBehaviour
    {
        //private int currentPlayerIndex = 0;
        private List<Player> players;
        public void Initialize(List<Player> players)
        {
            this.players = players;
        }
        public void StartTurn()
        {

        }
        public void EndTurn()
        {

        }
        public void DrawFor(Player player, DeckInstance drawingFromDeck)
        {

            CardInstance c = drawingFromDeck.DrawCard();
            player.AddCardToHand(c);

        }
        public void UseCardEffectOn(Player player, CardInstance card)
        {
            //todo
            if(card.cardData is RoleCardData roleCard)
            {
                //ApplyRoleCard(roleCard, player);
                player.SetPlayerRole(roleCard);
            }
            else if( card.cardData is CharacterCardData characterCard)
            {
                //ApplyCharacterCard(characterCard, player);
                player.SetPlayerCharacter(characterCard);
            }
            else if( card.cardData is PlayingCardData playingCard)
            {
                //ApplyPlayingCard(playingCard, player);
                
            }
        }
    }
}
