using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIMouseFollower : MonoBehaviour
{
    public RectTransform rectTransform;
    public TruckController truck;
    public TextMeshProUGUI text;
    public GameObject UIElement;
    private Equipment currentItem;

    private void Start()
    {
        currentItem = null;
    }

    void Update()
    {
        SetPosition();
        DisplayMouseoverInfo();
    }

    private void SetPosition()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 mousePos = Input.mousePosition;

        rectTransform.pivot = new Vector2(0, 0);

        if (mousePos.x + width > Screen.width)
        {
            rectTransform.pivot += new Vector2(1, 0);
        }
        if (mousePos.y + height > Screen.height)
        {
            rectTransform.pivot += new Vector2(0, 1);
        }

//        //Stops element from leaving the screen
//        mousePos = new Vector2(Mathf.Clamp(mousePos.x, 0, Screen.width - width), Mathf.Clamp(mousePos.y, 0, Screen.height - height));

        rectTransform.anchoredPosition = mousePos;
    }

    private void DisplayMouseoverInfo()
    {
        UIElement.SetActive(false);

        if (truck.GetMouseGridPosition().x >= 0)
        {
            Equipment equipment = truck.GetEquipmentStatsAtMouse();
            if (equipment != null)
            {
                UIElement.SetActive(true);
                text.text = "Cash: " + equipment.cashFlow + "\nHeat: " + equipment.thermalRating + "\nPower: " + equipment.power;
            }
        }
        else if(currentItem != null)
        {
            UIElement.SetActive(true);
            text.text = "Cash: " + currentItem.cashFlow + "\nHeat: " + currentItem.thermalRating + "\nPower: " + currentItem.power;
        }
    }

    public void SetCurrentItem(Equipment newItem)
    {
        currentItem = newItem;
    }

    public void ClearCurrentItem()
    {
        currentItem = null;
    }
}
