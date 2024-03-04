using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bang;
public class CardObject : MonoBehaviour
{
    [SerializeField] private Renderer frontRenderer;
    [SerializeField] private Renderer backRenderer;

    public void SetFrontMaterial(Material material)
    {
        frontRenderer.material = material;
    }
    public void SetBackMaterial(Material material)
    {
        backRenderer.material = material;
    }
}
