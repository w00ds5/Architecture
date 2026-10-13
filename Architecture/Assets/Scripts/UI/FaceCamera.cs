using UnityEngine;

[DefaultExecutionOrder(100)]
public class FaceCamera : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private void Awake()
    {
        if (cameraTransform == null)
        {
            Camera mainCamera = Camera.main;

            if (mainCamera != null)
                cameraTransform = mainCamera.transform;
        }

        if (cameraTransform == null)
        {
            Debug.LogError("Не найдена основная камера.", this);
            enabled = false;
        }
    }

    private void LateUpdate()
    {
        if (cameraTransform != null)
            transform.rotation = cameraTransform.rotation;
    }
}