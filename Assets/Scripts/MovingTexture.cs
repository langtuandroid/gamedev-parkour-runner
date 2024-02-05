using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTexture : MonoBehaviour
{
    public Vector2 moveDirection;

    public Renderer rendererer;
    private void Start()
    {
        rendererer = GetComponent<Renderer>();
    }
    private void Update()
    {
        rendererer.material.SetTextureOffset("_BaseMap", moveDirection + rendererer.material.GetTextureOffset("_BaseMap"));
        
    }
}
