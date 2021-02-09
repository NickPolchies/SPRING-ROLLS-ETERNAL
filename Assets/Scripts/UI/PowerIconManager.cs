using System.Collections.Generic;
using UnityEngine;

public class PowerIconManager : MonoBehaviour
{
    [SerializeField] private float spacing = 0;
    [SerializeField] Transform anchor = null;
    [SerializeField] int sortingLayer = 3;
    [SerializeField] float scale = 2;
    public Sprite powerSprite;
    public int maxSprites;
    public TMPro.TMP_FontAsset counterFont;
    public int counterFontSize = 24;
    private FloatingText floatingText;
    private List<SpriteRenderer> powerImageList;
    public int powerCount { get; private set; }

    void Start()
    {
        powerCount = 0;
        floatingText = new FloatingText(anchor.GetComponentInChildren<RectTransform>().localPosition, transform, counterFont, counterFontSize, TMPro.TextAlignmentOptions.Left);

        powerImageList = new List<SpriteRenderer>();
        AddPower();
    }

    public void AddPower()
    {
        powerCount++;
        if (powerCount <= maxSprites)
        {
            floatingText.DespawnText();

            GameObject newImageObject = new GameObject("BatteryImage" + powerImageList.Count);
            SpriteRenderer newBatteryImage = newImageObject.AddComponent<SpriteRenderer>();
            newImageObject.transform.position = anchor.position;
            newImageObject.transform.SetParent(transform);
            newImageObject.transform.localScale = Vector3.one * scale;
            newImageObject.transform.position += new Vector3(spacing * powerImageList.Count, 0, 0);
            newBatteryImage.sprite = powerSprite;
            newBatteryImage.sortingOrder = sortingLayer;

            powerImageList.Add(newBatteryImage);
        }
        else if(powerCount == maxSprites + 1)
        {
            HideIcons();
            floatingText.SpawnText("x" + powerCount, 100);
        }
        else
        {
            floatingText.SpawnText("x" + powerCount,100);
        }
    }

    public bool RemovePower()
    {
        if (powerCount <= 0)
        {
            return false;
        }

        powerCount--;
        if (powerCount < maxSprites)
        {
            SpriteRenderer renderer = powerImageList[powerCount];
            powerImageList.RemoveAt(powerCount);
            Destroy(renderer);
        }
        else if(powerCount == maxSprites)
        {
            floatingText.DespawnText();
            ShowIcons();
        }
        else
        {
            floatingText.SpawnText("x" + powerCount, 100);
        }

        return true;
    }

    public void OnDestroy()
    {
        PowerDown();
    }

    public void PowerDown()
    {
        while (RemovePower()) ;
    }

    private void HideIcons()
    {
        for (int i = 1; i < powerImageList.Count; i++)
        {
            powerImageList[i].enabled = false;
        }
    }

    private void ShowIcons()
    {
        for (int i = 1; i < powerImageList.Count; i++)
        {
            powerImageList[i].enabled = true;
        }
    }
}
