using UnityEngine;
using UnityEngine.InputSystem;

public class KeyboardMovementInput : MonoBehaviour, IMovementInput
{
    public Vector2 MoveDirection
    {
        get
        {
            Keyboard keyboard = Keyboard.current;

            if (keyboard == null || !isActiveAndEnabled)
                return Vector2.zero;

            float horizontal = 0f;
            float vertical = 0f;

            if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
                horizontal -= 1f;

            if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
                horizontal += 1f;

            if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
                vertical -= 1f;

            if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
                vertical += 1f;

            return new Vector2(horizontal, vertical).normalized;
        }
    }

    public bool RunRequested
    {
        get
        {
            Keyboard keyboard = Keyboard.current;

            return isActiveAndEnabled
                && keyboard != null
                && keyboard.shiftKey.isPressed;
        }
    }
}