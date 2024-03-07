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
            //InstantiateCards();
        }
        private void Start()
        {

        }
        private void InstantiateCards()
        {
            Debug.LogWarning("Instantiate Cards");
            //InstantiateRoles();
            //todo other cards
        }
        private void InstantiateRoles()
        {
            RolePile availableRoles = cardDatabase.GetAvailableRoles();
        }

    }
}

