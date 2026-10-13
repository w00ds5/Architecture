using UnityEngine;

public sealed class AnimationActionLock : StateMachineBehaviour
{
    public override void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.GetComponentInParent<CharacterActionLock>()
            ?.Lock(this);
    }

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.GetComponentInParent<CharacterActionLock>()
            ?.Unlock(this);
    }
}