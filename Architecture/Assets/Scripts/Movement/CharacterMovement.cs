using UnityEngine;

[RequireComponent(
    typeof(CharacterController),
    typeof(CharacterActionLock))]
public class CharacterMovement : MonoBehaviour
{
    [SerializeField, Min(0f)] private float walkSpeed = 4f;
    [SerializeField, Min(0f)] private float runSpeed = 7f;
    [SerializeField, Min(0f)] private float rotationSpeed = 720f;
    [SerializeField] private float gravity = -20f;

    private CharacterController _characterController;
    private float _verticalSpeed;
    private CharacterActionLock _actionLock;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _actionLock = GetComponent<CharacterActionLock>();
    }

    public void Move(Vector3 direction, bool runRequested)
    {
        direction.y = 0f;
        direction = Vector3.ClampMagnitude(direction, 1f);

        if (_actionLock.IsLocked)
            direction = Vector3.zero;

        UpdateGravity();

        float speed = runRequested ? runSpeed : walkSpeed;
        Vector3 velocity = direction * speed;
        velocity.y = _verticalSpeed;

        _characterController.Move(velocity * Time.deltaTime);

        RotateTowardsMovement(direction);
    }

    public void Face(Vector3 direction)
    {
        if (_actionLock.IsLocked)
            return;
            
        direction.y = 0f;
        RotateTowardsMovement(direction);
    }

    private void UpdateGravity()
    {
        if (_characterController.isGrounded && _verticalSpeed < 0f)
            _verticalSpeed = -2f;

        _verticalSpeed += gravity * Time.deltaTime;
    }

    private void RotateTowardsMovement(Vector3 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        transform.rotation = Quaternion.RotateTowards(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime);
    }
}