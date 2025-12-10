using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedTexture : PaintedTextureHook
{
    [SerializeField] private Texture sourceTexture;

    public override Texture GetTexture()
    {
        return sourceTexture;
    }

    public void SetTexture(Texture sourceTexture)
    {
        if (this.sourceTexture == null) { this.sourceTexture = sourceTexture; }
    }

    public static PaintedTexture CreateNew(Texture sourceTexture)
    {
        if(!Application.isPlaying) return null;

        GameObject obj = new GameObject();
        PaintedTexture res = obj.AddComponent<PaintedTexture>();
        res.SetTexture(sourceTexture);

        return res;
    }
}
