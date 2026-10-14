using System;
using UnityEngine;

[RequireComponent(typeof(CharacterHealth))]
public sealed class CharacterMana : MonoBehaviour
{
    [SerializeField, Min(1f)] private float maxMana = 100f;

    [SerializeField, Min(0f)]
    private float regenerationPerSecond = 2f;

    private ManaModel _model;
    private CharacterHealth _health;

    public float CurrentMana => GetModel().CurrentMana;
    public float MaxMana => GetModel().MaxMana;

    public event Action<float, float> Changed;

    private void Awake()
    {
        _health = GetComponent<CharacterHealth>();
        GetModel();
    }

    private void Update()
    {
        if (_health.IsDead)
            return;

        GetModel().Restore(
            regenerationPerSecond * Time.deltaTime);
    }

    public bool CanSpend(float amount)
    {
        return GetModel().CanSpend(amount);
    }

    public bool TrySpend(float amount)
    {
        return GetModel().TrySpend(amount);
    }

    public void RestoreState(float currentMana)
    {
        GetModel().SetCurrent(currentMana);
    }

    private ManaModel GetModel()
    {
        if (_model != null)
            return _model;

        _model = new ManaModel(maxMana);

        _model.Changed += (current, maximum) =>
            Changed?.Invoke(current, maximum);

        return _model;
    }
}