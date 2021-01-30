using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class TutorialController : MonoBehaviour
{
    public string mainGameSceneName;
    public TextMeshProUGUI playText;
    void Start()
    {
        StartCoroutine(Waitup());
    }
    void Update()
    {
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainGameSceneName);
        }
    }

    IEnumerator Waitup()
    {
        yield return new WaitForSeconds(3.5f);
        StartCoroutine(FlashCashText());
    }

    IEnumerator FlashCashText()
    {
        playText.color = new Color32(255, 255, 255, 255);
        yield return new WaitForSeconds(0.6f);
        playText.color = new Color32(0, 0, 0, 0);
        yield return new WaitForSeconds(0.6f);
            StartCoroutine(FlashCashText());
    }


}

