using UnityEngine;

[RequireComponent(typeof(MeleeAttack))]
public sealed class AttackAnimationDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int Melee = Animator.StringToHash("Melee");
    private static readonly int Cast = Animator.StringToHash("Cast");

    private MeleeAttack _meleeAttack;
    private MagicAttack _magicAttack;

    private void Awake()
    {
        _meleeAttack = GetComponent<MeleeAttack>();
        _magicAttack = GetComponent<MagicAttack>();
    }

    private void OnEnable()
    {
        _meleeAttack.AttackStarted += PlayMelee;

        if (_magicAttack != null)
            _magicAttack.AttackStarted += PlayCast;
    }

    private void OnDisable()
    {
        _meleeAttack.AttackStarted -= PlayMelee;

        if (_magicAttack != null)
            _magicAttack.AttackStarted -= PlayCast;
    }

    private void PlayMelee()
    {
        animator.ResetTrigger(Cast);
        animator.SetTrigger(Melee);
    }

    private void PlayCast()
    {
        animator.ResetTrigger(Melee);
        animator.SetTrigger(Cast);
    }
}