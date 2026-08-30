using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public string gameSceneName = "MainMenuScene";

    public void OnPlayButtonClicked()
    {
        SaveGameCoordinator.Instance.SaveGame();
        SceneManager.LoadScene(gameSceneName);
    }
}