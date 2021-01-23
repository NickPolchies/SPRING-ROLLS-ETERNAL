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
    private Equipment currentEquipment;
    private bool dragging;
    public Image equipmentImage;
    public float imageScale;
    private bool displayInfoPane;
    public string overpowerIncreaseTextColor, overpowerDecreaseTextColor;

    private void Start()
    {
        currentEquipType = null;
        currentEquipment = null;
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

        if(currentEquipType != null || currentEquipment != null)
        {
            equipmentImage.transform.position = Input.mousePosition;
        }

        if (displayInfoPane)
        {
            string cashText, heatText, powerText;
            cashText = "Cash: " + currentEquipType.CashFlow;
            heatText = "Heat: " + currentEquipType.Heat;
            powerText = "Power: " + currentEquipType.Power;

            if (currentEquipment && currentEquipment.powerStage > 1)
            {
                cashText += AppendScalingText(currentEquipType.CashFlowScaling, currentEquipment.powerStage);
                heatText += AppendScalingText(currentEquipType.HeatScaling, currentEquipment.powerStage);
                powerText += AppendScalingText(currentEquipType.PowerScaling, currentEquipment.powerStage);
            }

            mouseoverInfoPane.SetActive(true);
            nameText.text = currentEquipType.TypeName;
            statusText.text = cashText + "\n" + heatText + "\n" + powerText;
        }
    }

    private string AppendScalingText(float scaling, int powerStage)
    {
        powerStage--; //Base power stage doesn't count

        if (scaling > 0)
        {
            return "<color=" + overpowerIncreaseTextColor + "> +" + scaling * powerStage + "</color>";
        }
        else if (scaling < 0)
        {
            return "<color=" + overpowerDecreaseTextColor + "> " + scaling * powerStage + "</color>";
        }
        return "";
    }

    public void MouseEnter(EquipmentType e)
    {
        if (!dragging)
        {
            displayInfoPane = true;
            currentEquipType = e;
        }
    }

    public void MouseEnter(Equipment e)
    {
        if (dragging)
        {
            currentEquipment = null;
            return;
        }
        else
        {
            displayInfoPane = true;
            currentEquipment = e;
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
        currentEquipment = null;

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
