using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private MonoBehaviour inputSource;

    [SerializeField, Min(0.1f)] private float distance = 4f;
    [SerializeField] private float targetHeight = 1.5f;
    [SerializeField, Min(0f)] private float sensitivity = 0.15f;

    [SerializeField] private float minPitch = -10f;
    [SerializeField] private float maxPitch = 60f;

    private ILookInput _input;
    private float _yaw;
    private float _pitch = 20f;

    private void Awake()
    {
        _input = inputSource as ILookInput;
        _yaw = transform.eulerAngles.y;

        if (target == null || _input == null)
        {
            Debug.LogError(
                "Назначь Target и Input Source, реализующий ILookInput.",
                this);

            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (target == null)
            return;

        UpdateRotation();
        UpdatePosition();
    }

    private void UpdateRotation()
    {
        Vector2 lookDelta = _input.LookDelta;

        _yaw = Mathf.Repeat(
            _yaw + lookDelta.x * sensitivity,
            360f);

        _pitch = Mathf.Clamp(
            _pitch - lookDelta.y * sensitivity,
            minPitch,
            maxPitch);
    }

    private void UpdatePosition()
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 focusPoint = target.position + Vector3.up * targetHeight;
        Vector3 position = focusPoint + rotation * Vector3.back * distance;

        transform.SetPositionAndRotation(position, rotation);
    }
}