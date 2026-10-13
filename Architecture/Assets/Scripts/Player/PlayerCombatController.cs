using UnityEngine;

[RequireComponent(typeof(MeleeAttack), typeof(MagicAttack))]
public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private Transform cameraTransform;

    private ICombatInput _input;
    private MeleeAttack _meleeAttack;
    private MagicAttack _magicAttack;
    private CharacterActionLock _actionLock;

    private void Awake()
    {
        _actionLock = GetComponent<CharacterActionLock>();
        _input = inputSource as ICombatInput;
        _meleeAttack = GetComponent<MeleeAttack>();
        _magicAttack = GetComponent<MagicAttack>();

        if (_input == null || cameraTransform == null)
        {
            Debug.LogError(
                "Назначь Input Source с ICombatInput и Camera Transform.",
                this);

            enabled = false;
        }
    }

    private void Update()
    {
        if (_actionLock.MovementRequested)
            return;
        if (_input.PhysicalAttackPressed)
            _meleeAttack.TryAttack();

        if (_input.MagicalAttackPressed)
        {
            Vector3 direction = Vector3.ProjectOnPlane(
                cameraTransform.forward,
                Vector3.up).normalized;

            _magicAttack.TryAttack(direction);
        }
    }
}