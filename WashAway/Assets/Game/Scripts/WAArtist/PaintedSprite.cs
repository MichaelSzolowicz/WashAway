using UnityEngine;

[System.Serializable]
public class PaintedSprite
{
    [SerializeField] private PaintedTextureHook _paintedTextureHook;

    public Texture sourceTexture { 
        get { return _paintedTextureHook.GetTexture(); }
    }

    public Vector2 offset;
    public Vector2 scale;
    public string tag;
    public bool enabled = true;

    public PaintedSprite(PaintedTextureHook paintedTextureHook)
    {
        _paintedTextureHook = paintedTextureHook;
    }
}
