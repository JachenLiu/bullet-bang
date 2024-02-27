using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public class Duke : BaseCharacter
    {
        public Duke()
        {
            SetName("Lucky Duke");
            SetHealth(4);
            SetDescription("Each time he “draws!”, he flips the top 2 cards and chooses 1.");
        }
    }
}
