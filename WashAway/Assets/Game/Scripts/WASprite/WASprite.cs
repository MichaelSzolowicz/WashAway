using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASprite : MonoBehaviour
{
    [SerializeField] private Sprite colorSprite;
    [SerializeField] private Sprite normalSprite;
    [SerializeField] private Sprite roughSprite;
    [SerializeField] private Sprite thickSprite;

    [SerializeField, HideInInspector] private SpriteRenderer colorRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer normalRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer roughRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer thickRenderer;
}
