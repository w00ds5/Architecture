using System.Collections.Generic;
using UnityEngine;

public sealed class CharacterActionLock : MonoBehaviour
{
    private readonly HashSet<object> _owners = new();

    public bool MovementRequested { get; private set; }

    public bool IsLocked =>
        _owners.Count > 0 && !MovementRequested;

    public void SetMovementRequested(bool requested)
    {
        MovementRequested = requested;
    }

    public void Lock(object owner)
    {
        _owners.Add(owner);
    }

    public void Unlock(object owner)
    {
        _owners.Remove(owner);
    }
}