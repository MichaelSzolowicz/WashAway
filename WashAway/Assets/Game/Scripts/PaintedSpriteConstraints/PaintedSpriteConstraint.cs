using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintedSpriteConstraint : MonoBehaviour
{
    [SerializeField] private Transform parentTransform;
    [SerializeField] private WAArtist artist;
    [SerializeField] private string paintedSpriteTag;

    private PaintedSprite paintedSprite;

    private void BindSprite()
    {

    }
}
