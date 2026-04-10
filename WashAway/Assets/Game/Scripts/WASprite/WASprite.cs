using UnityEngine;

/// <summary>
/// Works in combination with WA Sprite Editor and Sprite Component Viewer to enable easy manipulation of a sprite with multiple layers.
/// </summary>
public class WASprite : MonoBehaviour
{
    [SerializeField] public SpriteComponentViewer colorRenderer;
    [SerializeField] public SpriteComponentViewer normalRenderer;
    [SerializeField] public SpriteComponentViewer rtmRenderer;
}
