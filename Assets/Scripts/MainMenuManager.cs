using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public TextMeshProUGUI currencyText;
    public TextMeshProUGUI energyText;
    public string gameSceneName = "GameScene";

    private void Start()
    {
        DisplayCurrency();
        DisplayEnergy();
    }

    private void DisplayCurrency()
    {

    }
    private void DisplayEnergy()
    {

    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
