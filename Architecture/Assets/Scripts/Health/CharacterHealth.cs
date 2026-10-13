using System;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField, Min(1)] private int maxHealth = 100;

    private HealthModel _model;

    public int MaxHealth => GetModel().MaxHealth;
    public int CurrentHealth => GetModel().CurrentHealth;
    public bool IsDead => GetModel().IsDead;

    public event Action<int, int> Changed;
    public event Action<int> Damaged;
    public event Action Died;

    private void Awake()
    {
        GetModel();
    }

    public void TakeDamage(int amount)
    {
        GetModel().TakeDamage(amount);
    }

    private HealthModel GetModel()
    {
        if (_model != null)
            return _model;

        _model = new HealthModel(maxHealth);

        _model.Changed += (current, maximum) =>
            Changed?.Invoke(current, maximum);

        _model.Damaged += amount =>
            Damaged?.Invoke(amount);

        _model.Died += () =>
            Died?.Invoke();

        return _model;
    }
}