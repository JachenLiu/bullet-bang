using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Kisch : SpecialCharacter
    {
        public Kisch()
        {
            SetName("Johnny Kisch");
            SetHealth(4);
            SetDescription("Each time he puts a card into play, all other cards in play with the same name are discarded.");
        }
    }
}
