using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class SceneRestartController : MonoBehaviour
{
    private bool _restarting;

    public void Restart()
    {
        if (_restarting)
            return;

        _restarting = true;
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(
            SceneManager.GetActiveScene().name);
    }
}