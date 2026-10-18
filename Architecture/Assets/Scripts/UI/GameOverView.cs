using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text titleText;

    public void Show()
    {
        Show("GAME OVER");
    }

    public void Show(string title)
    {
        titleText.text = title;
        panel.SetActive(true);
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}