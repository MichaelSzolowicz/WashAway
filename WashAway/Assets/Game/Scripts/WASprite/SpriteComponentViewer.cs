using System;
using UnityEngine;

/// <summary>
/// Exposes properties on a Sprite Renderer in another component's inspector using Sprite Component Drawer.
/// </summary>
[Serializable]
public class SpriteComponentViewer
{
    [SerializeField] public SpriteRenderer spriteRenderer;

    public SpriteComponentViewer(SpriteRenderer spriteRenderer)
    {
        this.spriteRenderer = spriteRenderer;
    }
}
