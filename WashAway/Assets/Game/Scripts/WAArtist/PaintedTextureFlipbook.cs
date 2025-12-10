using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedTextureFlipbook : PaintedTextureHook
{
    [SerializeField] private List<Texture> sourceTextures = new List<Texture>();

    public override Texture GetTexture()
    {
        return sourceTextures[0];
    }
}
