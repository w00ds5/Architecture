using UnityEngine;

public sealed class AttackStateExit : StateMachineBehaviour
{
    [SerializeField] private DamageType attackType;

    public override void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        animator.GetComponentInParent<CharacterAttackState>()
            ?.End(attackType);
    }
}