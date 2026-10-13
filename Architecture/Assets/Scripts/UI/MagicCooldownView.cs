using TMPro;
using UnityEngine;

public sealed class MagicCooldownView : MonoBehaviour
{
    [SerializeField] private MagicAttack magicAttack;
    [SerializeField] private RectTransform fill;
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        if (magicAttack == null || fill == null || label == null)
        {
            Debug.LogError(
                "Назначь Magic Attack, Fill и Label.",
                this);

            enabled = false;
            return;
        }

        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;

        Refresh();
    }

    private void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        float remaining = magicAttack.CooldownRemaining;
        float duration = Mathf.Max(
            0.01f,
            magicAttack.CooldownDuration);

        float fraction = Mathf.Clamp01(
            1f - remaining / duration);

        fill.anchorMax = new Vector2(fraction, 1f);
        label.text = $"Магия (ПКМ): {remaining:0.0} с";
    }
}