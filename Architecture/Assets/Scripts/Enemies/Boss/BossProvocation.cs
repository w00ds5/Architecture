using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public sealed class BossProvocation : MonoBehaviour
{
    private CharacterHealth _health;

    public bool IsProvoked { get; private set; }

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
    }

    private void OnEnable()
    {
        _health.Damaged += OnDamaged;
    }

    private void OnDisable()
    {
        if (_health != null)
            _health.Damaged -= OnDamaged;
    }

    public void RestoreState(bool provoked)
    {
        IsProvoked = provoked;
    }

    private void OnDamaged(int amount)
    {
        IsProvoked = true;
    }
}