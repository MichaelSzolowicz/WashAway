using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

// Change name to "PaintedSprite"?
[ExecuteInEditMode]
public class CanvasSprite : MonoBehaviour
{
    [SerializeField] private RenderTexture paintLayer;
    private RenderTexture paintBuffer;
    [SerializeField] private Texture sourceTexture;

    private Material blendMaterial;
    private CommandBuffer commandBuffer;

    private void Start()
    {
        Paint();
    }

    private void OnValidate()
    {
        if(blendMaterial == null)
        {
            blendMaterial = new Material(Shader.Find("Unlit/Blend"));
        }

        if(paintBuffer == null)
        {
            paintBuffer = new RenderTexture(paintLayer.width,  paintLayer.height, 0);
        }

        Paint();
    }

    private void Update()
    {
        Paint();
    }

    private void Paint()
    {
        commandBuffer = new CommandBuffer();

        Vector2 position = -transform.position;
        position.x *= transform.localScale.x;
        position.x -= (transform.localScale.x / 2) - .5f;
        position.y *= transform.localScale.y;
        position.y -= (transform.localScale.y / 2) - .5f;

        Graphics.SetRenderTarget(paintLayer);
        commandBuffer.ClearRenderTarget(true, true, Color.black);

        commandBuffer.Blit(sourceTexture, paintBuffer, scale:transform.localScale, offset:position);
        commandBuffer.Blit(paintBuffer, paintLayer, blendMaterial, 0);

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
    }
}
