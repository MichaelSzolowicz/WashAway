using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

}
