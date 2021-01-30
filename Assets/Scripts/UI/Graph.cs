using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Graph : MonoBehaviour
{
    [SerializeField] private Sprite point;
    [SerializeField] private RectTransform graphContainer;
    [SerializeField] private RectTransform labelTemplateX;
    [SerializeField] private RectTransform labelTemplateY;
    [SerializeField] private RectTransform lineTemplateX;
    [SerializeField] private RectTransform lineTemplateY;
    private int separatorCount = 10;
    [SerializeField] private GameOverUI gameOverUI;

    private List<GameObject> points;
    private List<GameObject> lineConnectors;
    private List<GameObject> labelsX;
    private List<GameObject> labelsY;
    private List<GameObject> linesX;
    private List<GameObject> linesY;

    private void Awake()
    {
        points = new List<GameObject>();
        lineConnectors = new List<GameObject>();
        labelsX = new List<GameObject>();
        labelsY = new List<GameObject>();
        linesX = new List<GameObject>();
        linesY = new List<GameObject>();
    }

    private void Update()
    {
        if (Input.anyKeyDown)
        {
            CloseGraph();
        }
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

    public void ShowGraph(List<float> valueList)
    {
        ShowGraph(valueList, "");
    }

    public void ShowGraph(List<float> valueList, string valueDisplayOptions)
    {
        float xScale = graphContainer.rect.width / valueList.Count;
        float graphHeight = graphContainer.rect.height;
        float yMax = 0;

        for (int i = 0; i < valueList.Count; i++)
        {
            yMax = Mathf.Max(valueList[i], yMax);
        }

        yMax *= 1.1f;

        //Might work?
        float scale = Mathf.Pow(10, Mathf.Floor(Mathf.Log10(yMax)) + 1) * 0.01f;
        yMax = scale * Mathf.Ceil((yMax / scale));

        GameObject lastPoint = null;
        for (int i = 0; i < valueList.Count; i++)
        {
            float xPos = xScale/2 + i * xScale;
            float yPos = valueList[i] / yMax * graphHeight;

            GameObject point = CreateCircle(new Vector2(xPos, yPos));
            points.Add(point);
            if(i > 0)
            {
                LinkGraph(lastPoint.GetComponent<RectTransform>().anchoredPosition, point.GetComponent<RectTransform>().anchoredPosition);
            }
            lastPoint = point;

            RectTransform labelX = Instantiate(labelTemplateX);
            labelsX.Add(labelX.gameObject);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition += new Vector2(xPos, 0);
            labelX.GetComponent<TextMeshProUGUI>().text = i.ToString();

            RectTransform lineX = Instantiate(lineTemplateX);
            linesX.Add(lineX.gameObject);
            lineX.SetParent(graphContainer, false);
            lineX.gameObject.SetActive(true);
            lineX.anchoredPosition += new Vector2(xPos, 0);
        }

        for(int i = 0; i <= separatorCount; i++)
        {
            RectTransform labelY = Instantiate(labelTemplateY);
            labelsY.Add(labelY.gameObject);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition += new Vector2(0, normalizedValue * graphHeight);
            labelY.GetComponent<TextMeshProUGUI>().text = (normalizedValue * yMax).ToString(valueDisplayOptions);

            if(i == 0)
            {
                continue;
            }

            RectTransform lineY = Instantiate(lineTemplateY);
            linesY.Add(lineY.gameObject);
            lineY.SetParent(graphContainer, false);
            lineY.gameObject.SetActive(true);
            lineY.anchoredPosition += new Vector2(0, normalizedValue * graphHeight);
        }
    }

    private void LinkGraph(Vector2 pointA, Vector2 pointB)
    {
        GameObject connector = new GameObject("GraphLine", typeof(Image));
        lineConnectors.Add(connector);
        //GameObject g = new GameObject("Circle", typeof(Image));
        connector.transform.SetParent(graphContainer, false);
        connector.GetComponent<Image>().color = new Color(1, 1, 1, 0.5f);
        RectTransform rectTransform = connector.GetComponent<RectTransform>();
        Vector2 dir = (pointB - pointA).normalized;
        float dist = Vector2.Distance(pointA, pointB);
        rectTransform.anchoredPosition = pointA + dir * dist / 2;
        rectTransform.sizeDelta = new Vector2(dist, 4);
        rectTransform.anchorMin = Vector2.zero;
        rectTransform.anchorMax = Vector2.zero;
        rectTransform.localEulerAngles = new Vector3(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg);
    }

    public void CloseGraph()
    {
        ClearObjectList(points);
        ClearObjectList(lineConnectors);
        ClearObjectList(labelsX);
        ClearObjectList(labelsY);
        ClearObjectList(linesX);
        ClearObjectList(linesY);

        gameOverUI.gameObject.SetActive(true);
        transform.parent.gameObject.SetActive(false);
    }

    private void ClearObjectList(List<GameObject> list)
    {
        foreach(GameObject item in list)
        {
            Destroy(item);
        }
    }
}
