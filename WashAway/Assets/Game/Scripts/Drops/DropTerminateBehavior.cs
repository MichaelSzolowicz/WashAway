using UnityEngine;

/// <summary>
/// Terminates animator game object when state exits.
/// </summary>
public class DropTerminateBehavior : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject);
    }
}
