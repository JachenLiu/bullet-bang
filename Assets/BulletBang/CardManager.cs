using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField]
        private List<RoleCardData> roleCards = new();

        [SerializeField]
        private List<CharacterCardData> characterCards = new();

        [SerializeField]
        private List<PlayingCardData> playingCards = new();

        public List<RoleCardData> GetRoleCards()
        {
            return roleCards;
        }
        public List<CharacterCardData> GetCharacterCards()
        {
            return characterCards;
        }
        public List<PlayingCardData> GetPlayingCards()
        {
            return playingCards;
        }

    }

}