using TMPro;
using UnityEngine;

public sealed class ManaView : MonoBehaviour
{
    [SerializeField] private CharacterMana mana;
    [SerializeField] private RectTransform fill;
    [SerializeField] private TMP_Text label;

    private void OnEnable()
    {
        if (mana == null || fill == null || label == null)
        {
            Debug.LogError("Назначь Mana, Fill и Label.", this);
            enabled = false;
            return;
        }

        fill.anchorMin = Vector2.zero;
        fill.anchorMax = Vector2.one;
        fill.offsetMin = Vector2.zero;
        fill.offsetMax = Vector2.zero;

        mana.Changed += Refresh;
        Refresh(mana.CurrentMana, mana.MaxMana);
    }

    private void OnDisable()
    {
        if (mana != null)
            mana.Changed -= Refresh;
    }

    private void Refresh(float current, float maximum)
    {
        float fraction = Mathf.Clamp01(current / maximum);
        fill.anchorMax = new Vector2(fraction, 1f);

        label.text =
            $"{Mathf.FloorToInt(current)} / " +
            $"{Mathf.FloorToInt(maximum)} MP";
    }
}