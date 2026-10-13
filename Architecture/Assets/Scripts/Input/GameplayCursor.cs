using UnityEngine;
using UnityEngine.InputSystem;

public class GameplayCursor : MonoBehaviour
{
    private void Update()
    {
        if (!Application.isFocused)
            return;

        Keyboard keyboard = Keyboard.current;

        if (keyboard != null && keyboard.escapeKey.wasPressedThisFrame)
        {
            SetCaptured(false);
            return;
        }

        Mouse mouse = Mouse.current;

        if (Cursor.lockState != CursorLockMode.Locked
            && mouse != null
            && mouse.leftButton.wasPressedThisFrame)
        {
            SetCaptured(true);
        }
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus)
            SetCaptured(false);
    }

    private void OnDisable()
    {
        SetCaptured(false);
    }

    private void SetCaptured(bool captured)
    {
        Cursor.lockState = captured
            ? CursorLockMode.Locked
            : CursorLockMode.None;

        Cursor.visible = !captured;
    }
}