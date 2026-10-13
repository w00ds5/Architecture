using UnityEngine;

[DefaultExecutionOrder(100)]
public sealed class LocomotionAnimationDriver : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private static readonly int Speed = Animator.StringToHash("Speed");
    private Vector3 _previousPosition;

    private void OnEnable()
    {
        _previousPosition = transform.position;
    }

    private void Update()
    {
        if (animator == null || Time.deltaTime <= 0f)
            return;

        Vector3 displacement = transform.position - _previousPosition;
        displacement.y = 0f;

        float movementSpeed = displacement.magnitude / Time.deltaTime;
        animator.SetFloat(Speed, movementSpeed, 0.1f, Time.deltaTime);

        _previousPosition = transform.position;
    }
}