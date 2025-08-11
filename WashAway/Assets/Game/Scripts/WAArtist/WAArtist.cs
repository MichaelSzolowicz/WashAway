using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[ExecuteInEditMode]
public class WAArtist : MonoBehaviour
{
    [SerializeField] private RenderTexture painting;
    [SerializeField] private List<PaintedSprite> layers = new List<PaintedSprite>();
    
    private RenderTexture layerBuffer;
    private CommandBuffer commandBuffer;
    private Material blendMaterial;

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        if(painting == null)
        {
            return;
        }

        layerBuffer = new RenderTexture(painting.width, painting.height, 0);
        commandBuffer = new CommandBuffer();
        blendMaterial = new Material(Shader.Find("Unlit/Blend"));
    }

    private void Update()
    {
        
    }

    private void Paint()
    {

    }
}
