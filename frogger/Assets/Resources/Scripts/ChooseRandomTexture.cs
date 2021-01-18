using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseRandomTexture : MonoBehaviour
{
    public Texture[] possibleTextures;

    [SerializeField]
    private Renderer objectRenderer;

    private void Start()
    {
        objectRenderer.material.mainTexture = possibleTextures[Random.Range(0, possibleTextures.Length)];
    }
}
