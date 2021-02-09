using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverUI : MonoBehaviour
{
    public TruckController truck;
    public bool gameOverHot;
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
        if (gameOverHot == true)
        {
            gameOverReasons.text = "OH NO, YOUR TRUCK MELTED!";
        }
        else
        {
            gameOverReasons.text = "OH NO, YOUR TRUCK FROZE!";
        }

        float highScore = PlayerPrefs.GetFloat("HighScoreCash", -1);
        if (highScore > 0)
        {
            stats.text += "\nPREVIOUS HIGH SCORE: " + highScore;
        }
        //Song.gameObject.SetActive(true);
    }

    void ReturnToMenu()
    {
        float highScore = PlayerPrefs.GetFloat("HighScoreCash", -1);
        if (truck.lifetimeCash > highScore)
        {
            PlayerPrefs.SetFloat("HighScoreCash", truck.lifetimeCash);
        }

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
