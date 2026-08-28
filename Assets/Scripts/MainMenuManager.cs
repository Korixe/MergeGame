using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameSceneName = "GameScene";

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}