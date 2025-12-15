using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerCharacterAnimationController : MonoBehaviour
{
    [SerializeField] private PlatformerMovement _platformerMovement;

    [SerializeField] private PaintedSpriteConstraintComponent _constraintColor;
    [SerializeField] private PaintedSpriteConstraintComponent _constraintNormal;
    [SerializeField] private PaintedSpriteConstraintComponent _constraintRough;
    [SerializeField] private PaintedSpriteConstraintComponent _constraintThickness;

    [SerializeField] private PaintedTextureHook _idleColor;
    [SerializeField] private PaintedTextureHook _idleNormal;
    [SerializeField] private PaintedTextureHook _idleRough;
    [SerializeField] private PaintedTextureHook _idleThickness;

    [SerializeField] private PaintedTextureHook _runColor;
    [SerializeField] private PaintedTextureHook _runNormal;
    [SerializeField] private PaintedTextureHook _runRough;
    [SerializeField] private PaintedTextureHook _runThickness;

    private enum State
    {
        Idle,
        Run
    }

    private State _state = State.Idle;

    private void Update()
    {
        switch (_state)
        {
            case State.Idle:
                _constraintColor.paintedSprite.paintedTextureHook = _idleColor;
                _constraintNormal.paintedSprite.paintedTextureHook = _idleNormal;
                _constraintRough.paintedSprite.paintedTextureHook = _idleRough;
                _constraintThickness.paintedSprite.paintedTextureHook = _idleThickness;

                if(_platformerMovement.walkSpeed > 0) _state = State.Run;
                break;
            case State.Run:
                _constraintColor.paintedSprite.paintedTextureHook = _runColor;
                _constraintNormal.paintedSprite.paintedTextureHook = _runNormal;
                _constraintRough.paintedSprite.paintedTextureHook = _runRough;
                _constraintThickness.paintedSprite.paintedTextureHook = _runThickness;

                if(_platformerMovement.walkSpeed <= 0) _state = State.Idle;
                break;
            default:
                break;
        }
    }
}
