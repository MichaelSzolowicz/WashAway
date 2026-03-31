using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEditor;
using UnityEngine;

public class WASprite : MonoBehaviour
{
    //[Header("Header")]
    [SerializeField] private SpriteComponentViewer colorRenderer;
    [SerializeField] private SpriteComponentViewer normalRenderer;
    //[Header("Header")]
    [SerializeField] private SpriteComponentViewer roughRenderer;
    [SerializeField] private SpriteComponentViewer thickRenderer;
}
