using System.Collections;
using UnityEngine;

public class PlayerGameOverController : MonoBehaviour
{
    [SerializeField] private CharacterHealth playerHealth;
    [SerializeField] private GameOverView gameOverView;
    [SerializeField] private GameplayCursor gameplayCursor;
    [SerializeField] private MonoBehaviour[] playerInputs;
    [SerializeField, Min(0f)] private float gameOverDelay = 4.7f;

    private Coroutine _showRoutine;
    private bool _isEnding;

    private void OnEnable()
    {
        if (playerHealth == null
            || gameOverView == null
            || gameplayCursor == null
            || playerInputs == null
            || playerInputs.Length == 0)
        {
            Debug.LogError(
                "Назначь здоровье, экран поражения, курсор и ввод игрока.",
                this);

            enabled = false;
            return;
        }

        _isEnding = false;
        gameOverView.Hide();
        playerHealth.Died += EndGame;

        if (playerHealth.IsDead)
            EndGame();
    }

    private void OnDisable()
    {
        if (playerHealth != null)
            playerHealth.Died -= EndGame;

        if (_showRoutine != null)
        {
            StopCoroutine(_showRoutine);
            _showRoutine = null;
        }
    }

    private void EndGame()
    {
        if (_isEnding)
            return;

        _isEnding = true;

        foreach (MonoBehaviour input in playerInputs)
        {
            if (input != null)
                input.enabled = false;
        }

        gameplayCursor.enabled = false;
        _showRoutine = StartCoroutine(ShowGameOverAfterDeath());
    }

    private IEnumerator ShowGameOverAfterDeath()
    {
        yield return new WaitForSeconds(gameOverDelay);
        _showRoutine = null;
        gameOverView.Show();
    }
}