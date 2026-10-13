using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLookInput : MonoBehaviour, ILookInput
{
    public Vector2 LookDelta
    {
        get
        {
            Mouse mouse = Mouse.current;

            if (!isActiveAndEnabled
                || !Application.isFocused
                || Cursor.lockState != CursorLockMode.Locked
                || mouse == null)
            {
                return Vector2.zero;
            }

            return mouse.delta.ReadValue();
        }
    }
}