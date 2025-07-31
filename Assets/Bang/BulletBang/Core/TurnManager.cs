using ExitGames.Client.Photon;
using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class TurnManager : NetworkBehaviour
    {
        private PlayerManager _playerManager;

        public void Initialize(PlayerManager playerManager)
        {
            _playerManager = playerManager;
        }
        private void Start()
        {
            StartTurn();
        }
        private TurnPhase _currentPhase;

        public void StartTurn()
        {
            _currentPhase = TurnPhase.DrawPhase;
            NextPhase();
        }
        public void NextPhase()
        {
            switch (_currentPhase)
            {
                case TurnPhase.DrawPhase:
                    //draw
                    break;
                case TurnPhase.PlayPhase:
                    //play
                    break;
                case TurnPhase.DiscardPhase:
                    //discard
                    break;
            }
        }
        public void StartPlayerTurn(Player player)
        {
            if (player.HasStateAuthority)
            {
                //draw
            }
        }
        [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
        public void RPC_PlayCard(PlayerRef player, NetworkId cardId)
        {
            //Player player = PlayerManager.Instance.GetPlayer(PlayerRef)
        }


        private int currentPlayerIndex;
        private int totalPlayers;

        private void Awake()
        {
        }


        public void EndTurn()
        {
            Debug.Log($"Ending Player {currentPlayerIndex + 1}'s Turn");
            currentPlayerIndex = (currentPlayerIndex + 1) % totalPlayers;
            StartTurn();
        }
        public void SetTotalPlayers(int numberOfPlayers)
        {
            totalPlayers = numberOfPlayers;
        }

        //public void Initialize(List<Player> players)
        //{
        //    this.players = players;
        //}
        //public void StartTurn()
        //{
        //    //Player currentPlayer = players[currentPlayerIndex];
        //    //currentPlayerIndex++;
        //    //if(currentPlayerIndex >= players.Count)
        //    //{
        //    //    currentPlayerIndex = 0;
        //    //}
        //    //DrawFor(currentPlayer, currentPlayer.GetPlayerHand());
        //}
        //public void EndTurn()
        //{

        //}
        //public void DrawFor(Player player, DeckInstance drawingFromDeck)
        //{

        //    CardInstance c = drawingFromDeck.DrawCard();
        //    player.AddCardToHand(c);

        //}
        //public void UseCardEffectOn(Player player, CardInstance card)
        //{
        //    //todo
        //    if(card.cardData is RoleCardData roleCard)
        //    {
        //        //ApplyRoleCard(roleCard, player);
        //        player.SetPlayerRole(roleCard);
        //    }
        //    else if( card.cardData is CharacterCardData characterCard)
        //    {
        //        //ApplyCharacterCard(characterCard, player);
        //        player.SetPlayerCharacter(characterCard);
        //    }
        //    else if( card.cardData is PlayingCardData playingCard)
        //    {
        //        //ApplyPlayingCard(playingCard, player);
                
        //    }
        //}
    }
}
