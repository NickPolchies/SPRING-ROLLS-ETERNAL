using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class HighScoreDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    float highScore;
    private Button button;
    private TextMeshProUGUI highScoreTextBox;

    string resetScoreText = "RESET HIGH SCORE";
    string highScoreText = "HIGH SCORE: ";

    void Start()
    {
        highScore = PlayerPrefs.GetFloat("HighScoreCash", -1);
        highScoreTextBox = GetComponentInChildren<TextMeshProUGUI>();
        highScoreText += highScore;
        highScoreTextBox.text = highScoreText;

        button = GetComponent<Button>();
        button.onClick.AddListener(ResetHighScore);

        if (highScore < 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        highScoreTextBox.text = resetScoreText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        highScoreTextBox.text = highScoreText;
    }

    void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScoreCash");
        gameObject.SetActive(false);
    }
}
