using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TruckController truck;
    public string mainMenuSceneName;
    public Button mainMenuButton;
    public TextMeshProUGUI stats;

    void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMenu);
        stats.text = "YOU EARNED A TOTAL OF " + truck.lifetimeCash + " DOLLARS";
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
