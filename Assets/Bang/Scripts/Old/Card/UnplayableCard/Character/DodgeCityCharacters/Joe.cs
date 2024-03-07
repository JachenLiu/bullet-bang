using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Joe : DodgeCityCharacter
    {
        public Joe()
        {
            SetName("Tequila Joe");
            SetHealth(4);
            SetDescription("Each time he plays a Beer, he regains 2 health instead of 1.");
        }
    }
}
