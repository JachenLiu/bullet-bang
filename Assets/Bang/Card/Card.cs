using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public interface Card
    {
        GameObject GameObject { get; }
        public abstract bool IsPlayable();
        public void ViewCard()
        {

        }
        //setMaterial
        void SetFrontMaterial(Material material);
        void SetBackMaterial(Material material);
    }
}