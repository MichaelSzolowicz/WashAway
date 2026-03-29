using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WASprite : MonoBehaviour
{
    [SerializeField, HideInInspector] private SpriteRenderer colorRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer normalRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer roughRenderer;
    [SerializeField, HideInInspector] private SpriteRenderer thickRenderer;
}
