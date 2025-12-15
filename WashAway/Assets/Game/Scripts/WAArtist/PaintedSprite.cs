using UnityEngine;

[System.Serializable]
public class PaintedSprite
{
    [SerializeField] public PaintedTextureHook paintedTextureHook;

    public Texture sourceTexture { 
        get { return paintedTextureHook.GetTexture(); }
    }

    public Vector2 offset;
    public Vector2 scale;
    public string tag;
    public bool enabled = true;

    public PaintedSprite(PaintedTextureHook paintedTextureHook)
    {
        this.paintedTextureHook = paintedTextureHook;
    }
}
