using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PauseMenuInput : MonoBehaviour, IPauseMenuInput
{
    public bool PausePressed
    {
        get
        {
            Keyboard keyboard = Keyboard.current;

            return isActiveAndEnabled
                && Application.isFocused
                && keyboard != null
                && keyboard.escapeKey.wasPressedThisFrame;
        }
    }
}