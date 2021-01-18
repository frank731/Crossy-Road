using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingTexture : MonoBehaviour
{
    public float speedX = 0;
    public float speedY = 0;
    private float currX;
    private float currY;
    private Renderer textureRenderer;

    private void Start()
    {
        textureRenderer = GetComponent<Renderer>();
        currX = textureRenderer.material.mainTextureOffset.x;
        currY = textureRenderer.material.mainTextureOffset.y;
    }

    private void FixedUpdate()
    {
        currX += Time.deltaTime * speedX;
        currY += Time.deltaTime * speedY;
        textureRenderer.material.mainTextureOffset = new Vector2(currX, currY);
    }
}
