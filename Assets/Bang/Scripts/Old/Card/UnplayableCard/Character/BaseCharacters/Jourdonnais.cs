using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Jourdonnais : BaseCharacter
    {
        public Jourdonnais()
        {
            SetName("Jourdonnais");
            SetHealth(4);
            SetDescription("Whenever he is the target of a BANG!, he may “draw!”: on a heart, he is missed.");
        }
    }
}
