using UnityEngine;
using UnityEngine.InputSystem;

public class MouseCombatInput : MonoBehaviour, ICombatInput
{
    private bool CanReadInput =>
        isActiveAndEnabled
        && Application.isFocused
        && Cursor.lockState == CursorLockMode.Locked;

    public bool PhysicalAttackPressed
    {
        get
        {
            Mouse mouse = Mouse.current;

            return CanReadInput
                && mouse != null
                && mouse.leftButton.wasPressedThisFrame;
        }
    }

    public bool MagicalAttackPressed
    {
        get
        {
            Mouse mouse = Mouse.current;

            return CanReadInput
                && mouse != null
                && mouse.rightButton.wasPressedThisFrame;
        }
    }
}