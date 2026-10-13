using UnityEngine;

[RequireComponent(typeof(CharacterMovement))]
public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] private MonoBehaviour inputSource;
    [SerializeField] private Transform cameraTransform;

    private IMovementInput _input;
    private CharacterMovement _movement;

    private void Awake()
    {
        _input = inputSource as IMovementInput;
        _movement = GetComponent<CharacterMovement>();

        if (_input == null || cameraTransform == null)
        {
            Debug.LogError(
                "Назначь Input Source с IMovementInput и Camera Transform.",
                this);

            enabled = false;
        }
    }

    private void Update()
    {
        Vector2 inputDirection = _input.MoveDirection;
        Vector3 movementDirection = GetMovementDirection(inputDirection);

        _movement.Move(movementDirection, _input.RunRequested);
    }

    private Vector3 GetMovementDirection(Vector2 inputDirection)
    {
        Vector3 forward = Vector3.ProjectOnPlane(
            cameraTransform.forward,
            Vector3.up).normalized;

        Vector3 right = Vector3.ProjectOnPlane(
            cameraTransform.right,
            Vector3.up).normalized;

        return forward * inputDirection.y
            + right * inputDirection.x;
    }
}