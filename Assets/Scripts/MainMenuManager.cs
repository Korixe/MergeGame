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
        UpdateEnergyText(EnergyManager.Instance.energyAmount);
        EnergyManager.Instance.OnEnergyChanged += UpdateEnergyText;
        DisplayEnergy();
    }

    private void UpdateEnergyText(int newEnergyAmount)
    {
        energyText.text = newEnergyAmount.ToString();
    }
    private void DisplayEnergy()
    {

    }

    public void OnPlayButtonClicked()
    {
        SceneManager.LoadScene(gameSceneName);
    }
    private void OnDestroy()
    {  
        if (EnergyManager.Instance != null)
            EnergyManager.Instance.OnEnergyChanged -= UpdateEnergyText;
    }
}