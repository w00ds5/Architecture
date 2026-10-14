using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class UnitySceneLoader : ISceneLoader
{
    public bool IsLoading { get; private set; }

    public void Load(string sceneName)
    {
        if (IsLoading)
            return;

        if (!Application.CanStreamedLevelBeLoaded(sceneName))
        {
            Debug.LogError(
                $"Сцена '{sceneName}' недоступна. Проверь Scene List.");

            return;
        }

        IsLoading = true;

        AsyncOperation operation =
            SceneManager.LoadSceneAsync(sceneName);

        operation.completed += _ => IsLoading = false;
    }
}