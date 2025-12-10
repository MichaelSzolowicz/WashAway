using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedTextureFlipbook : MonoBehaviour, IPaintedTextureHook
{
    [SerializeField] private List<Texture> sourceTextures = new List<Texture>();

    public Texture GetTexture()
    {
        return sourceTextures[0];
    }
}
