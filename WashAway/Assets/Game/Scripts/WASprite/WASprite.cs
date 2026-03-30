using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class WASprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer colorRenderer;
    [SerializeField] private SpriteRenderer normalRenderer;
    [SerializeField] private SpriteRenderer roughRenderer;
    [SerializeField] private SpriteRenderer thickRenderer;
}
