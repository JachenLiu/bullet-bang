using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Will : SpecialCharacter
    {
        public Will()
        {
            SetName("Uncle Will");
            SetHealth(4);
            SetDescription("Once during his turn, he may play any card from hand as a General Store.");
        }
    }
}
