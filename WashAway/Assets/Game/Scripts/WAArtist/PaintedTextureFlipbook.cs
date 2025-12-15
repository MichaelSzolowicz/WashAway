using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedTextureFlipbook : PaintedTextureHook
{
    [SerializeField] private List<Texture> sourceTextures = new List<Texture>();
    [SerializeField] private int frequency = 12;
    private int currentFrame = 0;
    private float elapsedTime = 0;

    public override Texture GetTexture()
    {
        if (!Application.isPlaying)
        {
            elapsedTime = 0.0f;
            currentFrame = 0;
            return sourceTextures[currentFrame];
        }

        elapsedTime += Time.deltaTime;
        float min = (1.0f / frequency);
        if (elapsedTime >= min) 
        {
            currentFrame++;
            if (currentFrame >= sourceTextures.Count) { currentFrame = 0; }
            elapsedTime = 0;
        }

        return sourceTextures[currentFrame];
    }
}
