using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BulletBang
{
    public class CardInstance : MonoBehaviour
    {
        public CardData cardData;

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
