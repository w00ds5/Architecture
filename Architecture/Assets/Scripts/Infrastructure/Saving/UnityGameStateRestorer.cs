using System;
using UnityEngine;

public sealed class UnityGameStateRestorer
{
    private readonly ICharacterSaveState _player;
    private readonly IEnemyFactory _enemyFactory;

    public UnityGameStateRestorer(
        ICharacterSaveState player,
        IEnemyFactory enemyFactory)
    {
        _player = player
            ?? throw new ArgumentNullException(nameof(player));

        _enemyFactory = enemyFactory
            ?? throw new ArgumentNullException(nameof(enemyFactory));
    }

    public void Restore(GameSaveData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        _player.Restore(data.player);

        foreach (EnemySaveData enemy in data.enemies)
        {
            CharacterSaveData state = enemy.state;

            ICharacterSaveState character = _enemyFactory.Create(
                enemy.kind,
                new Vector3(
                    state.positionX,
                    state.positionY,
                    state.positionZ),
                state.rotationY);

            character.Restore(state);
        }

        Physics.SyncTransforms();
    }
}