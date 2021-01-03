using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsController : MonoBehaviour
{
    public string mainMenuSceneName;
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuSceneName);
        }
    }
}
