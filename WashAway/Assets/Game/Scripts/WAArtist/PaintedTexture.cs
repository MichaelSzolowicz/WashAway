using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedTexture : MonoBehaviour, IPaintedTextureHook
{
    [SerializeField] private Texture sourceTexture;

    public Texture GetTexture()
    {
        return sourceTexture;
    }
}
