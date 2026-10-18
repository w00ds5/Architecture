using System;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public sealed class UnityCharacterSaveState :
    MonoBehaviour, ICharacterSaveState
{
    private CharacterHealth _health;
    private CharacterMana _mana;
    private MeleeAttack _meleeAttack;
    private MagicAttack _magicAttack;
    private CharacterController _characterController;
    private BossProvocation _bossProvocation;
    private BossStrongAttack _bossStrongAttack;

    public bool IsAlive =>
        this != null
        && isActiveAndEnabled
        && _health != null
        && !_health.IsDead;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        _mana = GetComponent<CharacterMana>();
        _meleeAttack = GetComponent<MeleeAttack>();
        _magicAttack = GetComponent<MagicAttack>();

        _characterController =
            GetComponent<CharacterController>();

        _bossProvocation = GetComponent<BossProvocation>();
        _bossStrongAttack = GetComponent<BossStrongAttack>();
    }

    public CharacterSaveData Capture()
    {
        Vector3 position = transform.position;

        return new CharacterSaveData
        {
            positionX = position.x,
            positionY = position.y,
            positionZ = position.z,

            rotationY = transform.eulerAngles.y,

            health = _health.CurrentHealth,
            mana = _mana != null ? _mana.CurrentMana : 0f,

            meleeCooldown = _meleeAttack != null
                ? _meleeAttack.CooldownRemaining
                : 0f,

            magicCooldown = _magicAttack != null
                ? _magicAttack.CooldownRemaining
                : 0f,

            bossProvoked = _bossProvocation != null
                && _bossProvocation.IsProvoked,

            strongAttackCooldown = _bossStrongAttack != null
                ? _bossStrongAttack.CooldownRemaining
                : 0f
        };
    }

    public void Restore(CharacterSaveData data)
    {
        if (data == null)
            throw new ArgumentNullException(nameof(data));

        RestoreTransform(data);

        _health.RestoreState(data.health);

        if (_mana != null)
            _mana.RestoreState(data.mana);

        if (_meleeAttack != null)
            _meleeAttack.RestoreCooldown(data.meleeCooldown);

        if (_magicAttack != null)
            _magicAttack.RestoreCooldown(data.magicCooldown);

        if (_bossProvocation != null)
            _bossProvocation.RestoreState(data.bossProvoked);

        if (_bossStrongAttack != null)
        {
            _bossStrongAttack.RestoreCooldown(
                data.strongAttackCooldown);
        }
    }

    private void RestoreTransform(CharacterSaveData data)
    {
        bool controllerWasEnabled =
            _characterController != null
            && _characterController.enabled;

        if (controllerWasEnabled)
            _characterController.enabled = false;

        try
        {
            transform.SetPositionAndRotation(
                new Vector3(
                    data.positionX,
                    data.positionY,
                    data.positionZ),
                Quaternion.Euler(0f, data.rotationY, 0f));
        }
        finally
        {
            if (controllerWasEnabled)
                _characterController.enabled = true;
        }
    }
}