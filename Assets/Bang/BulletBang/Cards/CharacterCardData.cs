using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{

    // Subclass for Character cards
    [CreateAssetMenu(fileName = "New Character Card", menuName = "Cards/Character Card")]
    public class CharacterCardData : CardData
    {
        [SerializeField]
        private CharacterType characterName;

        [SerializeField]
        private int characterHealth;

        [SerializeField]
        private string characterDescription;

        public CharacterType GetCharacter()
        {
            return characterName;
        }
        public int GetCharacterHealth()
        {
            return characterHealth;
        }
        public string GetCharacterDescription()
        {
            return characterDescription;
        }
    }
}