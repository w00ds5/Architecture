using System;

public sealed class GameSaveDataValidator
{
    public void Validate(GameSaveData data)
    {
        if (data == null)
            throw new InvalidOperationException("Сохранение пустое.");

        if (data.version != GameSaveData.CurrentVersion)
        {
            throw new InvalidOperationException(
                "Неподдерживаемая версия сохранения.");
        }

        ValidateCharacter(data.player);

        if (data.enemies == null)
        {
            throw new InvalidOperationException(
                "Отсутствует список мобов.");
        }

        foreach (EnemySaveData enemy in data.enemies)
        {
            if (enemy == null
                || !Enum.IsDefined(typeof(EnemyKind), enemy.kind))
            {
                throw new InvalidOperationException(
                    "Некорректный тип моба.");
            }

            ValidateCharacter(enemy.state);
        }
    }

    private static void ValidateCharacter(CharacterSaveData state)
    {
        if (state == null
            || state.health <= 0
            || !IsFinite(state.positionX)
            || !IsFinite(state.positionY)
            || !IsFinite(state.positionZ)
            || !IsFinite(state.rotationY)
            || !IsFinite(state.mana)
            || !IsFinite(state.meleeCooldown)
            || !IsFinite(state.magicCooldown)
            || state.mana < 0f
            || state.meleeCooldown < 0f
            || state.magicCooldown < 0f)
        {
            throw new InvalidOperationException(
                "Некорректное состояние персонажа.");
        }
    }

    private static bool IsFinite(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value);
    }
}