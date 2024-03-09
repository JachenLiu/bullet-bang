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

        //public CardInstance cardPrefab;

        public List<RoleCardData> GetRoleCardsData()
        {
            return roleCards;
        }
        public List<CharacterCardData> GetCharacterCardsData()
        {
            return characterCards;
        }
        public List<PlayingCardData> GetPlayingCardsData()
        {
            return playingCards;
        }

        //public void TestCard()
        //{
        //    foreach (RoleCardData roleCardData in roleCards)
        //    {
        //        InstantiateCardData(roleCardData, GetRandomPosition());
        //    }
        //    foreach (CharacterCardData characterCardData in characterCards)
        //    {
        //        InstantiateCardData(characterCardData, GetRandomPosition());
        //    }
        //    foreach(PlayingCardData playingCardData in playingCards)
        //    {
        //        InstantiateCardData(playingCardData, GetRandomPosition());
        //    }
        //}

        //private void InstantiateCardData(CardData cardData, Vector3 position)
        //{
        //    CardInstance cardInstance = Instantiate(cardPrefab, position, Quaternion.identity, transform);
        //    cardInstance.SetCardData(cardData);
        //}
        //private Vector3 GetRandomPosition()
        //{
        //    return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f);
        //}

    }

        

    }