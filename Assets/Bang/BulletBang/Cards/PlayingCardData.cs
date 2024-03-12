using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{

    [CreateAssetMenu(fileName = "New Playing Card", menuName = "Cards/Playing Card")]
    public class PlayingCardData : CardData
    {
        [SerializeField]
        private CardNumber number;

        [SerializeField]
        private CardSuit suit;

        [SerializeField]
        private PlayingCardType playingCardType;

        [SerializeField]
        private PlayingCardName PlayingCardName;

        public CardNumber GetCardNumber()
        {
            return number;
        }
        public CardSuit GetCardSuit()
        {
            return suit;
        }
        public PlayingCardType GetPlayingCardType()
        {
            return playingCardType;
        }
        public PlayingCardName GetPlayingCardName()
        {
            return PlayingCardName;
        }

    }
}