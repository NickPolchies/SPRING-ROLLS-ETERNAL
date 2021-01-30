using System.Collections.Generic;
using UnityEngine;

public class BatteryIconManager : MonoBehaviour
{
    [SerializeField] private float spacing;
    [SerializeField] Transform anchor;
    [SerializeField] int sortingLayer = 3;
    [SerializeField] float scale = 2;
    public Sprite batterySprite;

    private Stack<SpriteRenderer> batteryImageList;

    void Start()
    {
        batteryImageList = new Stack<SpriteRenderer>();
        AddBattery();
    }

    public int Count {
        get { return batteryImageList.Count; } 
    }
    
    public void AddBattery()
    {
        GameObject newImageObject = new GameObject("BatteryImage" + batteryImageList.Count);
        SpriteRenderer newBatteryImage = newImageObject.AddComponent<SpriteRenderer>();
        newImageObject.transform.position = anchor.position;
        newImageObject.transform.SetParent(transform);
        newImageObject.transform.localScale = Vector3.one * scale;
        newImageObject.transform.position += new Vector3(spacing * batteryImageList.Count, 0, 0);
        newBatteryImage.sprite = batterySprite;
        newBatteryImage.sortingOrder = sortingLayer;

        batteryImageList.Push(newBatteryImage);
    }

    public bool RemoveBattery()
    {
        if (batteryImageList.Count <= 0)
        {
            return false;
        }
        Destroy(batteryImageList.Pop());
        return true;
    }

    public void OnDestroy()
    {
        RemoveAll();
    }

    public void RemoveAll()
    {
        while (RemoveBattery()) ;
    }
}
