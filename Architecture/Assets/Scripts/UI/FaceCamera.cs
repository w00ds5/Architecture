using UnityEngine;

[DefaultExecutionOrder(100)]
public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Назначь Camera Transform.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
            transform.rotation = cameraTransform.rotation;
    }
}