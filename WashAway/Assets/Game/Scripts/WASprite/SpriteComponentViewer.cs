using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SpriteComponentViewer
{
    [SerializeField] public SpriteRenderer spriteRenderer;

    public SpriteComponentViewer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }
}
