using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
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