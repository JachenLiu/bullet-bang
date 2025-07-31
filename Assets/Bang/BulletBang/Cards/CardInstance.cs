using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class CardInstance : NetworkBehaviour
    {
        public CardData cardData { get; private set; }
        public bool cardInPlay {  get; private set; }

        public void SetCardData(CardData cardData)
        {
            this.cardData = cardData;
            foreach(MeshRenderer mat in GetComponentsInChildren<MeshRenderer>())
            {
                if(mat.name == "Card Front")
                {
                    mat.material = cardData.frontMaterial;
                }
                if (mat.name == "Card Back")
                {
                    mat.material = cardData.backMaterial;
                }
            }
        }

    }
}
