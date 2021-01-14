using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite point;
    private RectTransform graphContainer;

    private void Awake()
    {
        graphContainer = GetComponent<RectTransform>();

        //CreateCircle(new Vector2(200, 400));
        List<float> values = new List<float>() {1f, 2f, 5f, 2.5f, 7f, 3f, 5.5f, 6.5f, 3.3334f };
        ShowGraph(values);
    }

    private GameObject CreateCircle(Vector2 anchoredPosition)
    {
        GameObject g = new GameObject("Circle", typeof(Image));
        g.transform.SetParent(graphContainer, false);
        g.GetComponent<Image>().sprite = point;
        RectTransform rectTransform = g.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = new Vector2(11, 11);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        return g;
    }

    private void ShowGraph(List<float> valueList)
    {
        float xScale = graphContainer.rect.width / valueList.Count;
        float graphHeight = graphContainer.rect.height;
        float yMax = 0;

        for (int i = 0; i < valueList.Count; i++)
        {
            yMax = Mathf.Max(valueList[i], yMax);
        }

        yMax *= 1.1f;

        GameObject lastPoint = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xScale/2 + i * xScale;
            float yPos = valueList[i] / yMax * graphHeight;

            Debug.Log(xPos + ", " + yPos);
            GameObject point = CreateCircle(new Vector2(xPos, yPos));
            if(i > 0)
            {
                LinkGraph(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition);
            }
            lastPoint = point;
        }
    }

    private void LinkGraph(Vector2 pointA, Vector2 pointB)
    {
        GameObject g = new GameObject("GraphLine", typeof(Image));
        //GameObject g = new GameObject("Circle", typeof(Image));
        g.transform.SetParent(graphContainer, false);
        g.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = g.GetComponent<RectTransform>();
        Vector2 dir = (pointB - pointA).normalized;
        float dist = Vector2.Distance(pointA, pointB);
        rectTransform.anchoredPosition = pointA + dir * dist / 2;
        rectTransform.sizeDelta = new Vector2(dist, 4);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }
}
