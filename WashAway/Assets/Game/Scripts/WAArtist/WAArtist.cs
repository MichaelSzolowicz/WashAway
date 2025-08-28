using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteAlways]
public class WAArtist : MonoBehaviour
{
    [SerializeField] protected RenderTexture painting;
    [SerializeField] public List<PaintedSprite> layers = new List<PaintedSprite>();
    [SerializeField] protected string blendShader = "Unlit/Blend";
    [SerializeField] protected bool additive = false;
    [SerializeField] protected bool clearOnStart = false;
    [SerializeField] protected bool clearOnDestroy = false;
 
    protected RenderTexture layerBuffer;
    protected CommandBuffer commandBuffer;
    protected Material blendMaterial;

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

    protected void Start()
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
        if(Application.isPlaying)
        {
            if (GameState.Paused) return;
            if (GameState.CurrentLevelClear) return;
        }

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

    protected void OnDestroy()
    {
        if (clearOnDestroy)
        {
            Graphics.SetRenderTarget(painting);
            commandBuffer.ClearRenderTarget(true, true, Color.clear);
            Graphics.ExecuteCommandBuffer(commandBuffer);
        }
    }
}
