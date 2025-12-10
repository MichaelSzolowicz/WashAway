using UnityEngine;

[System.Serializable]
public class PaintedSprite
{
    [SerializeField] private IPaintedTextureHook paintedTextureHook;

    public Texture sourceTexture { get { return paintedTextureHook.GetTexture(); } }
    public Vector2 offset;
    public Vector2 scale;
    public string tag;
    public bool enabled = true;
}
