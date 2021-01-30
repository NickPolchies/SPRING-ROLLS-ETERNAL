using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TruckController truck;
    public bool gameOverType;
    public string mainMenuSceneName;
    public Button mainMenuButton;
    public Button cashGraphButton;
    public Button heatGraphButton;
    public TextMeshProUGUI stats;
    public TextMeshProUGUI gameOverReasons;
    public Graph graph;
    public StatusUI statusUI;
    public GameObject Song;

    //TODO update this UI in general, not specifically this script
    void Start()
    {
        mainMenuButton.onClick.AddListener(ReturnToMenu);
        cashGraphButton.onClick.AddListener(DisplayCashGraph);
        heatGraphButton.onClick.AddListener(DisplayHeatGraph);
        stats.text = "YOU EARNED A TOTAL OF " + truck.lifetimeCash + " DOLLARS";
        if (gameOverType == true)
        {
            gameOverReasons.text = "Oh no, your truck melted!";
        }
        else
        {
            gameOverReasons.text = "Oh no, your truck froze!";
        }    
        //Song.gameObject.SetActive(true);
    }

    void ReturnToMenu()
    {
        SceneManager.LoadScene(mainMenuSceneName);
    }

    void DisplayCashGraph()
    {
        graph.transform.parent.gameObject.SetActive(true);
        graph.ShowGraph(statusUI.dailyTotalCash, "c0");
        gameObject.SetActive(false);
    }

    void DisplayHeatGraph()
    {
        graph.transform.parent.gameObject.SetActive(true);
        graph.ShowGraph(statusUI.dailyOutsideTemperature, "F1");
        gameObject.SetActive(false);
    }
}
