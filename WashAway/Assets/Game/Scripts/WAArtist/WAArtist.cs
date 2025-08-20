using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class WAArtist : MonoBehaviour
{
    [SerializeField] private RenderTexture painting;
    [SerializeField] public List<PaintedSprite> layers = new List<PaintedSprite>();
    [SerializeField] private string blendShader = "Unlit/Blend";
    [SerializeField] private bool additive = false;
    [SerializeField] private bool clearOnStart = false;
    [SerializeField] private bool clearOnDestroy = false;
 
    private RenderTexture layerBuffer;
    private CommandBuffer commandBuffer;
    private Material blendMaterial;

    public int Size 
    { 
        get 
        { 
            if(painting == null) return 0;
            return painting.width; 
        } 
    }

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        Init();

        if (clearOnStart)
        {
            Graphics.SetRenderTarget(painting);
            commandBuffer.ClearRenderTarget(true, true, Color.clear);
            Graphics.ExecuteCommandBuffer(commandBuffer);
        }
    }

    private void Init()
    {
        if(painting == null)
        {
            return;
        }

        layerBuffer = new RenderTexture(painting.width, painting.height, 0);
        commandBuffer = new CommandBuffer();
        blendMaterial = new Material(Shader.Find(blendShader));
    }

    private void Update()
    {
#if UNITY_EDITOR
        if(Application.isPlaying)
        {
            if (GameState.Paused) return;
            if (GameState.CurrentLevelClear) return;
        }
#endif

        Paint();
    }

    private void Paint()
    {
        if (painting == null)
        {
            return;
        }

        if(!additive)
        {
            Graphics.SetRenderTarget(painting);
            commandBuffer.ClearRenderTarget(true, true, Color.clear);
        }

        foreach (PaintedSprite layer in layers)
        {
            commandBuffer.Blit(layer.sourceTexture, layerBuffer, scale: layer.scale, offset: layer.offset);
            commandBuffer.Blit(layerBuffer, painting, blendMaterial, 0);
        }

        Graphics.ExecuteCommandBuffer(commandBuffer);
        commandBuffer.Clear();
        Graphics.SetRenderTarget(null);
    }

    public PaintedSprite FindSpriteByTag(string tag)
    {
        PaintedSprite sprite = null;

        foreach (PaintedSprite layer in layers)
        {
            if(layer.tag == tag)
            {
                sprite = layer;
            }
        }

        return sprite;
    }

    public void AddLayer(PaintedSprite layer)
    {
        layers.Add(layer);
    }

    private void OnDestroy()
    {
        if (clearOnDestroy)
        {
            Graphics.SetRenderTarget(painting);
            commandBuffer.ClearRenderTarget(true, true, Color.clear);
            Graphics.ExecuteCommandBuffer(commandBuffer);
        }
    }
}
