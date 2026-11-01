using UnityEngine;

public class DropStateBehaviorAnimateTrail : StateMachineBehaviour
{
    [SerializeField] private GameObject trailPrefab;
    private GameObject trailInstance;
    private DropTrail trailComponent;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       trailInstance = Instantiate(trailPrefab, animator.transform.position, Quaternion.identity);
       trailComponent = trailInstance.GetComponent<DropTrail>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        trailComponent.UpdateAnimation(stateInfo.normalizedTime);
    }
}
