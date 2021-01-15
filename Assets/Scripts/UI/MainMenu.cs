using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Button startGame;
    public Button credits;
    public Button quitGame;
    public string mainSceneName;
    public string creditsSceneName;

    void Start()
    {
        startGame.onClick.AddListener(StartGame);
        credits.onClick.AddListener(Credits);
        quitGame.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        SceneManager.LoadScene(mainSceneName);
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
