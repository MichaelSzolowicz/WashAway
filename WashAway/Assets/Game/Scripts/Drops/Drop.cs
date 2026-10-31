using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drop : MonoBehaviour
{
    [SerializeField] private DropTrail dropTrail;
    [SerializeField] private Animator dropAnimator;

    private int fallStateName = Animator.StringToHash("DropFall");

    private void Update()
    {
        if(dropAnimator == null) return;

        AnimatorStateInfo stateInfo = dropAnimator.GetCurrentAnimatorStateInfo(0);

        Debug.Log(stateInfo.shortNameHash.ToString());

        if(stateInfo.shortNameHash == fallStateName)
        {
            dropTrail.UpdateAnimation(stateInfo.normalizedTime);
        }
    }
}
