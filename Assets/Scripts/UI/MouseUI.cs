using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MouseUI : MonoBehaviour
{
    public RectTransform rectTransform;
    public TruckController truck;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI statusText;
    public GameObject mouseoverInfoPane;
    private EquipmentType currentEquipType;
    private bool dragging;
    public Image equipmentImage;
    public float imageScale;
    private bool displayInfoPane;

    private void Start()
    {
        currentEquipType = null;
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

        rectTransform.anchoredPosition = mousePos;
    }

    private void DisplayMouseoverInfo()
    {
        mouseoverInfoPane.SetActive(false);

        if(currentEquipType != null)
        {
            equipmentImage.transform.position = Input.mousePosition;
        }

        if (displayInfoPane)
        {
            mouseoverInfoPane.SetActive(true);
            nameText.text = currentEquipType.TypeName;
            statusText.text = "Cash: " + currentEquipType.CashFlow + "\nHeat: " + currentEquipType.Heat + "\nPower: " + currentEquipType.Power;
        }
    }

    public void MouseEnter(EquipmentType e)
    {
        if (!dragging)
        {
            displayInfoPane = true;
            currentEquipType = e;
        }
    }

    public void MouseExit()
    {
        if (!dragging)
        {
            displayInfoPane = false;
            //currentItem = null;
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

        truck.BuyEquipment(truck.GetMouseGridPosition(), currentEquipType);
    }
}
