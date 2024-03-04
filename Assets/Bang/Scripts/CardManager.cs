using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class CardManager : MonoBehaviour
    {
        private CardDatabase cardDatabase;

        private void Awake()
        {
            InstantiateCards();
        }
        private void Start()
        {
            
        }
        private void InstantiateCards()
        {
            InstantiateRoles();
        }
        private void InstantiateRoles()
        {

        }

    }
}

