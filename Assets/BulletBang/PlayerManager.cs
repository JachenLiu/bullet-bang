using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class PlayerManager : MonoBehaviour
    {
        public Player playerPrefab;

        private List<Player> players = new();

        public void TestPlayers()
        {
            // Create Player 1
            Player player1 = Instantiate(playerPrefab, transform);
            player1.SetPlayerName("Player 1");
            players.Add(player1);

            //// Create Player 2
            Player player2 = Instantiate(playerPrefab);
            player2.SetPlayerName("Player 2");
            players.Add(player2);

            InitializePlayers(players);
        }
        
        public void InitializePlayers(List<Player> players)
        {
            this.players = players;
        }
        public List<Player> GetPlayers()
        {
            return players;
        }
        public Player GetPlayer(int index)
        {
            return players[index];
        }
        public int GetPlayerCount()
        {
            return players.Count;
        }
        public void Print()
        {
            for (int i = 0; i < players.Count; i++)
            {
                Player player = players[i];
                Debug.Log("Player " + (i) + ":");
                Debug.Log("  Name: " + player.GetPlayerName());
                Debug.Log("  Health: " + player.GetPlayerHealth() + " / " + player.GetPlayerMaxHealth());
                Debug.Log("Character: " + player.GetPlayerCharacter());
                Debug.Log("Role : " + player.GetPlayerRole());

                // Add more information as needed
            }
        }
    }
}

