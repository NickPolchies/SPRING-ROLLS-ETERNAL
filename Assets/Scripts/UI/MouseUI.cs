using System.Collections;
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
        else
        {
            DisplayTruckEquipment();
        }

        if (displayInfoPane)
        {
            Debug.Log("DISPLAY");
            mouseoverInfoPane.SetActive(true);
            text.text = "Cash: " + currentItem.cashFlow + "\nHeat: " + currentItem.thermalRating + "\nPower: " + currentItem.power;
        }
    }

    public void DisplayTruckEquipment()
    {
        Equipment equipment = truck.GetEquipmentAtMouse();
        if (equipment != null)
        {
            currentItem = equipment;
            displayInfoPane = true;
        }
        else
        {
            displayInfoPane = false;
        }
    }

    public void MouseEnter(Equipment e)
    {
        if (!dragging)
        {
            Debug.Log("ENTER");
            displayInfoPane = true;
            currentItem = e;
        }
    }

    public void MouseExit()
    {
        if (!dragging)
        {
            Debug.Log("EXIT");
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
        //        Debug.Log("drag start" + temp.ToString());

        dragging = true;
        displayInfoPane = true;

        //Debug.Log(temp.sprite);

//        dragImage.sprite = temp.sprite; //TODO swap the sprite rather than instantiate/destroy
    }

    public void DragStop()
    {
        dragging = false;
        displayInfoPane = false;
        equipmentImage.enabled = false;

        Debug.Log("Drag End: " + currentItem.name);

        truck.BuyEquipment(truck.GetMouseGridPosition(), currentItem);
    }
}
