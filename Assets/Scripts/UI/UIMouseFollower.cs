using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMouseFollower : MonoBehaviour
{
    public RectTransform rectTransform;

    void Update()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        rectTransform.anchoredPosition = Input.mousePosition;
    }
}
