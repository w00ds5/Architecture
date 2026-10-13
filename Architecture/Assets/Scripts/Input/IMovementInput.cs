using UnityEngine;

public interface IMovementInput
{
    Vector2 MoveDirection { get; }
    bool RunRequested { get; }
}