using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class UnityGameStateSource : IGameStateSource
{
    private readonly ICharacterSaveState _player;
    private readonly Transform _enemyParent;

    public UnityGameStateSource(
        ICharacterSaveState player,
        Transform enemyParent)
    {
        _player = player
            ?? throw new ArgumentNullException(nameof(player));

        if (enemyParent == null)
            throw new ArgumentNullException(nameof(enemyParent));

        _enemyParent = enemyParent;
    }

    public GameSaveData Capture()
    {
        if (!_player.IsAlive)
        {
            throw new InvalidOperationException(
                "Нельзя сохранить игру после смерти игрока.");
        }

        List<EnemySaveData> enemies = new();

        EnemySaveIdentity[] identities =
            _enemyParent.GetComponentsInChildren<EnemySaveIdentity>();

        foreach (EnemySaveIdentity enemy in identities)
        {
            ICharacterSaveState state = enemy.State;

            if (state == null || !state.IsAlive)
                continue;

            enemies.Add(new EnemySaveData
            {
                kind = enemy.Kind,
                state = state.Capture()
            });
        }

        return new GameSaveData
        {
            player = _player.Capture(),
            enemies = enemies.ToArray()
        };
    }
}