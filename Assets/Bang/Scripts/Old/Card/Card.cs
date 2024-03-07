using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bang
{
    public interface Card
    {
        //GameObject GameObject { get; }
        public abstract bool IsPlayable();
        public void ViewCard()
        {
            Debug.LogWarning("viewing card");
        }
        //setMaterial
        //void SetFrontMaterial(Material material);
        //void SetBackMaterial(Material material);
    }
}