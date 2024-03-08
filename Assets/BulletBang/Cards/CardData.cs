using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    [CreateAssetMenu(fileName = "New Card", menuName = "Cards/Card")]
    public class CardData : ScriptableObject
    {
        public string cardId = System.Guid.NewGuid().ToString();
        public string cardName;
        public string description;
        public CardType cardType;
        public Material frontMaterial;
        public Material backMaterial;

    }
}