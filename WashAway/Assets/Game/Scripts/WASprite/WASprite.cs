using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class WASprite : MonoBehaviour
{
    [SerializeField] public SpriteComponentViewer colorRenderer;
    [SerializeField] public SpriteComponentViewer normalRenderer;
    [SerializeField] public SpriteComponentViewer roughRenderer;
    [SerializeField] public SpriteComponentViewer thickRenderer;
}
