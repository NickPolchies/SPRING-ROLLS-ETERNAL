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

    void Update()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 mousePos = Input.mousePosition;
        //Vector2 pivot = new Vector2(0, 0);

        //Stops element from leaving the screen
        mousePos = new Vector2(Mathf.Clamp(mousePos.x, 0, Screen.width - width), Mathf.Clamp(mousePos.y, 0, Screen.height - height));

        rectTransform.anchoredPosition = mousePos;

        /*
        if (mousePos.x + width > Screen.width)
        {
            pivot += Vector2.right;
        }
        if(mousePos.y + height > Screen.height)
        {
            pivot += Vector2.up;
        }
        */

        //rectTransform.pivot = pivot;


        DisplayMouseoverInfo();
    }

    private void DisplayMouseoverInfo()
    {
        UIElement.SetActive(false);

        if (truck.GetMouseGridPosition().x >= 0)
        {
            Equipment equipment = truck.GetEquipmentStatsAtMouse();
            Debug.Log(equipment);
            if (equipment != null)
            {
                UIElement.SetActive(true);
                text.text = "Cash: " + equipment.cashFlow + "\nHeat: " + equipment.thermalRating + "\nPower: " + equipment.power;
            }
        }
    }
}
