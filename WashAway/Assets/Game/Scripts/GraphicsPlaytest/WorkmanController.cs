using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkmanController : MonoBehaviour
{
    [SerializeField] private Transform workman;
    [SerializeField] private Transform moveTo;
    [SerializeField] private float speed;
    [SerializeField] private GameObject lineCollider;

    [Header("Color")]
    [SerializeField] private PaintedSpriteConstraintComponent colorConstraint;
    [SerializeField] private PaintedTextureHook colorReactionAnim;
    [SerializeField] private PaintedTextureHook colorRunAnim;

    [Header("normal")]
    [SerializeField] private PaintedSpriteConstraintComponent normalConstraint;
    [SerializeField] private PaintedTextureHook normalReactionAnim;
    [SerializeField] private PaintedTextureHook normalRunAnim;

    [Header("rough")]
    [SerializeField] private PaintedSpriteConstraintComponent roughConstraint;
    [SerializeField] private PaintedTextureHook roughReactionAnim;
    [SerializeField] private PaintedTextureHook roughRunAnim;

    [Header("thickness")]
    [SerializeField] private PaintedSpriteConstraintComponent thicknessConstraint;
    [SerializeField] private PaintedTextureHook thicknessReactionAnim;
    [SerializeField] private PaintedTextureHook thicknessRunAnim;


    [SerializeField] private float reactionDelay = .5f;

    private bool destinationReached = false;

    public void StartRunAway()
    {
        StartCoroutine(RunAwayCoroutine());
    }

    private IEnumerator RunAwayCoroutine()
    {
        lineCollider.SetActive(false);

        colorConstraint.paintedSprite.paintedTextureHook = colorReactionAnim;
        normalConstraint.paintedSprite.paintedTextureHook = normalReactionAnim;
        roughConstraint.paintedSprite.paintedTextureHook = roughReactionAnim;
        thicknessConstraint.paintedSprite.paintedTextureHook = thicknessReactionAnim;

        yield return new WaitForSeconds(reactionDelay);

        colorConstraint.paintedSprite.paintedTextureHook = colorRunAnim;
        normalConstraint.paintedSprite.paintedTextureHook = normalRunAnim;
        roughConstraint.paintedSprite.paintedTextureHook = roughRunAnim;
        thicknessConstraint.paintedSprite.paintedTextureHook = thicknessRunAnim;

        while (!destinationReached)
        {
            Vector3 direction = (moveTo.position - workman.position).normalized;

            Vector3 newPosition = workman.position + direction * speed * Time.deltaTime;
            
            if(Vector3.Dot((moveTo.position - newPosition).normalized, direction) <= 0)
            {
                workman.position = moveTo.position;
                destinationReached = true;
            }
            else
            {
                workman.position = newPosition;
            }

            yield return new WaitForSeconds(0); 
        }
    }

}
