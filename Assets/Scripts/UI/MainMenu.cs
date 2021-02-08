using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGame;
    public Button credits;
    public Button quitGame;
    public Button tutorialButton;
    public Button highScoreButton;
    public string mainSceneName;
    public string creditsSceneName;
    public string tutorialSceneName;


    void Start()
    {
        startGame.onClick.AddListener(StartGame);
        tutorialButton.onClick.AddListener(ViewTutorial);
        credits.onClick.AddListener(Credits);
        quitGame.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        if (PlayerPrefs.GetInt("TutorialViewed", 0) > 0)
        {
            SceneManager.LoadScene(mainSceneName);
        }
        else
        {
            PlayerPrefs.SetInt("TutorialViewed", 1);
            SceneManager.LoadScene(tutorialSceneName);
        }

    }

    void ViewTutorial()
    {
        SceneManager.LoadScene(tutorialSceneName);
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.OpenURL(webplayerQuitURL);
#else
        Application.Quit();
#endif
    }

    void Credits()
    {
        SceneManager.LoadScene(creditsSceneName);
    }
}
