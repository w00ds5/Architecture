using TMPro;
using UnityEngine;

public sealed class HealthTextView : MonoBehaviour
{
    [SerializeField] private CharacterHealth health;
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        if (health == null || label == null)
        {
            Debug.LogError("Назначь Health и Label.", this);
            enabled = false;
            return;
        }

        health.Changed += Refresh;
        Refresh(health.CurrentHealth, health.MaxHealth);
    }

    private void OnDisable()
    {
        if (health != null)
            health.Changed -= Refresh;
    }

    private void Refresh(int current, int maximum)
    {
        label.text = $"{current} / {maximum}";
    }
}