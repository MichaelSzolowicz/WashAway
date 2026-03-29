using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WASprite : MonoBehaviour
{
    [SerializeField] private Sprite colorSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite roughSprite;
    [SerializeField] private Sprite thickSprite;

    // TODO: visible for testing purpose, should be hidden in inspector.
    [SerializeField] private SpriteRenderer colorRenderer;
    [SerializeField] private SpriteRenderer normalRenderer;
    [SerializeField] private SpriteRenderer roughRenderer;
    [SerializeField] private SpriteRenderer thickRenderer;

    private void OnEnable()
    {
        UpdateAllRenderers();
    }

    public void UpdateAllRenderers()
    {
        UpdateRenderer(colorRenderer, colorSprite, "Color");
        UpdateRenderer(normalRenderer, normalSprite, "Normal");
        UpdateRenderer(roughRenderer, roughSprite, "Rough");
        UpdateRenderer(thickRenderer, thickSprite, "Thick");
    }

    private void UpdateRenderer(SpriteRenderer renderer, Sprite sprite, string layer)
    {
        if (renderer == null) return;

        renderer.gameObject.layer = LayerMask.NameToLayer(layer);
        renderer.sprite = sprite;
    }


    private void OnValidate()
    {
        Debug.Log("Validate");
    }
}
