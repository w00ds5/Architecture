using UnityEngine;

public class HealthBarView : MonoBehaviour
{
    [SerializeField] private CharacterHealth health;
    [SerializeField] private RectTransform fill;

    private void OnEnable()
    {
        if (health == null || fill == null)
        {
            Debug.LogError("Назначь Health и Fill.", this);
            enabled = false;
            return;
        }

        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;

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
        float fraction = Mathf.Clamp01((float)current / maximum);
        fill.anchorMax = new Vector2(fraction, 1f);
    }
}