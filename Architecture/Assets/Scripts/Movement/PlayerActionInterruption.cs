using UnityEngine;

[DefaultExecutionOrder(-100)]
[RequireComponent(
    typeof(CharacterHealth),
    typeof(CharacterActionLock))]
public sealed class PlayerActionInterruption : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private Animator animator;

    private static readonly int MoveRequested =
        Animator.StringToHash("MoveRequested");

    private static readonly int Melee = Animator.StringToHash("Melee");
    private static readonly int Cast = Animator.StringToHash("Cast");
    private static readonly int Hurt = Animator.StringToHash("Hurt");

    private IMovementInput _input;
    private CharacterHealth _health;
    private CharacterActionLock _actionLock;
    private bool _wasMoving;

    private void Awake()
    {
        _input = inputSource as IMovementInput;
        _health = GetComponent<CharacterHealth>();
        _actionLock = GetComponent<CharacterActionLock>();

        if (_input == null || animator == null)
        {
            Debug.LogError("Назначь ввод движения и Animator.", this);
            enabled = false;
        }
    }

    private void Update()
    {
        bool moving = !_health.IsDead
            && _input.MoveDirection.sqrMagnitude > 0.001f;

        _actionLock.SetMovementRequested(moving);
        animator.SetBool(MoveRequested, moving);

        if (moving || _wasMoving)
        {
            animator.ResetTrigger(Melee);
            animator.ResetTrigger(Cast);
            animator.ResetTrigger(Hurt);
        }

        _wasMoving = moving;
    }

    private void OnDisable()
    {
        if (_actionLock != null)
            _actionLock.SetMovementRequested(false);

        if (animator != null)
            animator.SetBool(MoveRequested, false);

        _wasMoving = false;
    }
}