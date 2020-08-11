﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public TruckController truck;
    public TextMeshProUGUI text;
    public GameObject mouseoverInfoPane;
    private Equipment currentItem;
    private bool dragging;
    public Image equipmentImage;
    public float imageScale;
    private bool displayInfoPane;

    private void Start()
    {
        currentItem = null;
        dragging = false;
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
        mouseoverInfoPane.SetActive(false);

        if (currentItem != null)
        {
            equipmentImage.transform.position = Input.mousePosition;
        }

//        DisplayTruckEquipment();

        if (displayInfoPane)
        {
            mouseoverInfoPane.SetActive(true);
            text.text = "Cash: " + currentItem.cashFlow + "\nHeat: " + currentItem.thermalRating + "\nPower: " + currentItem.power;
        }
    }

    public void DisplayTruckEquipment()
    {
        Equipment equipment = truck.GetEquipmentAtMouse();

        if (dragging || currentItem != null) { }

        if (equipment != null)
        {
            currentItem = equipment;
            displayInfoPane = true;
        }
        else
        {
            displayInfoPane = false;
            currentItem = null;
        }
    }

    public void MouseEnter(Equipment e)
    {
        if (!dragging)
        {
            displayInfoPane = true;
            currentItem = e;
        }
    }

    public void MouseExit()
    {
        if (!dragging)
        {
            displayInfoPane = false;
            currentItem = null;
        }
    }

    public void DragStart(Sprite newSprite)
    {
        equipmentImage.sprite = newSprite;
        equipmentImage.SetNativeSize();
        equipmentImage.rectTransform.sizeDelta *= imageScale;
        equipmentImage.enabled = true;

        dragging = true;
        displayInfoPane = true;
    }

    public void DragStop()
    {
        dragging = false;
        displayInfoPane = false;
        equipmentImage.enabled = false;

        truck.BuyEquipment(truck.GetMouseGridPosition(), currentItem);
    }
}