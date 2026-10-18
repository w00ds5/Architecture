using System;
using System.Collections.Generic;

public sealed class EnemyDefeatTracker : IDisposable
{
    private readonly Dictionary<CharacterHealth, Action> _alive = new();

    public bool AllDefeated => _alive.Count == 0;

    public EnemyDefeatTracker(IEnumerable<CharacterHealth> enemies)
    {
        foreach (CharacterHealth enemy in enemies)
        {
            if (enemy == null
                || enemy.IsDead
                || _alive.ContainsKey(enemy))
            {
                continue;
            }

            Action handler = () => OnEnemyDied(enemy);

            _alive.Add(enemy, handler);
            enemy.Died += handler;
        }
    }

    public void Dispose()
    {
        foreach (var entry in _alive)
        {
            if (entry.Key != null)
                entry.Key.Died -= entry.Value;
        }

        _alive.Clear();
    }

    private void OnEnemyDied(CharacterHealth enemy)
    {
        if (!_alive.TryGetValue(enemy, out Action handler))
            return;

        enemy.Died -= handler;
        _alive.Remove(enemy);
    }
}