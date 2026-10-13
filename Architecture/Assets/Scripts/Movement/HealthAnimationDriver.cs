using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public sealed class HealthAnimationDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int Hurt = Animator.StringToHash("Hurt");
    private static readonly int Die = Animator.StringToHash("Die");

    private CharacterHealth _health;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        _health.Damaged += PlayHurt;
        _health.Died += PlayDeath;
    }

    private void OnDisable()
    {
        _health.Damaged -= PlayHurt;
        _health.Died -= PlayDeath;
    }

    private void PlayHurt(int amount)
    {
        if (_health.IsDead)
            return;

        animator.ResetTrigger("Melee");
        animator.ResetTrigger("Cast");
        animator.SetTrigger(Hurt);
    }

    private void PlayDeath()
    {
        animator.ResetTrigger(Hurt);
        animator.ResetTrigger("Melee");
        animator.ResetTrigger("Cast");
        animator.SetTrigger(Die);
    }
}