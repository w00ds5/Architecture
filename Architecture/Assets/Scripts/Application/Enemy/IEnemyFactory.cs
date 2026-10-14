using UnityEngine;

public interface IEnemyFactory
{
    ICharacterSaveState Create(
        EnemyKind kind,
        Vector3 position,
        float rotationY);
}