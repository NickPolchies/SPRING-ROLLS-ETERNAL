using TMPro;
using UnityEngine;

public class FloatingText
{
    private TextMeshProUGUI cashText;
    private Vector3 spawnPoint;
    private float remainingTime, totalTime;
    private Vector3 velocity;

    //TODO second pass
    public FloatingText(Vector3 anchorPoint, Transform parent, TMP_FontAsset font, int fontSize, TextAlignmentOptions alignment)
    {
        velocity = new Vector3(0f, 0.01f, 0f);
        spawnPoint = anchorPoint;

        GameObject cashObject = new GameObject("FloatingText", typeof(TextMeshProUGUI), typeof(Canvas));

        cashObject.GetComponent<Canvas>().sortingOrder = 10;

        cashObject.transform.SetParent(parent);

        cashText = cashObject.GetComponent<TextMeshProUGUI>();
        cashText.rectTransform.localScale = new Vector3(0.02f, 0.02f, 0.02f);
        cashText.alignment = alignment;
        cashText.enabled = true;
        cashText.transform.localPosition = spawnPoint;
        cashText.font = font;
        cashText.fontSize = fontSize;

        AdjustPositionToAlignment(alignment);
    }

    public void SpawnText(string text, float duration)
    {
        cashText.text = text;
        cashText.transform.localPosition = spawnPoint;

        totalTime = remainingTime = duration;
        cashText.alpha = 1;
    }

    public void DespawnText()
    {
        cashText.alpha = 0;
    }

    public void UpdateText(float deltaTime)
    {
        if(cashText.alpha <= 0)
        {
            return;
        }

        cashText.transform.localPosition += velocity;
        remainingTime -= deltaTime;
        float t = 1 - remainingTime / totalTime;

        t = t > 1 ? 1 : t*t;

        cashText.alpha = 1 - t;
    }

    private void AdjustPositionToAlignment(TextAlignmentOptions alignment)
    {
        Vector3 adjustment = Vector3.zero;

        switch (alignment)
        {
            case TextAlignmentOptions.Left:
                adjustment = new Vector3(cashText.rectTransform.rect.width, 0);
                break;
            case TextAlignmentOptions.BottomLeft:
                adjustment = new Vector3(cashText.rectTransform.rect.width, cashText.rectTransform.rect.height);
                break;
            case TextAlignmentOptions.TopLeft:
                adjustment = new Vector3(cashText.rectTransform.rect.width, -cashText.rectTransform.rect.height);
                break;
            case TextAlignmentOptions.Right:
                adjustment = new Vector3(-cashText.rectTransform.rect.width, 0);
                break;
            case TextAlignmentOptions.BottomRight:
                adjustment = new Vector3(-cashText.rectTransform.rect.width, cashText.rectTransform.rect.height);
                break;
            case TextAlignmentOptions.TopRight:
                adjustment = new Vector3(-cashText.rectTransform.rect.width, -cashText.rectTransform.rect.height);
                break;
        }

        cashText.rectTransform.localPosition += adjustment;
    }
}
