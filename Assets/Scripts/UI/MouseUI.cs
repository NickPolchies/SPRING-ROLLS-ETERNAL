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

    //Grid size numbers
    private Vector2 buildHighlightOffset = new Vector2(160, 192);
    private Vector2 buildHighlightSize = new Vector2(167.5f, 152.5f);
    private Vector2 buildTileSize = new Vector2(162.5f, 191.5f);
    public RectTransform buildHighlight;


    private void Start()
    {
        currentEquipType = null;
        currentEquipment = null;
        dragging = false;
        buildHighlight.gameObject.SetActive(false);
    }

    void Update()
    {
        SetPosition();
        DisplayMouseoverInfo();
        HighlightBuildLocation();
    }

    private void SetPosition()
    {
        float width = rectTransform.rect.width;
        float height = rectTransform.rect.height;

        Vector2 mousePos = Input.mousePosition;
        mousePos = new Vector2(mousePos.x / Screen.width * 1280, mousePos.y / Screen.height * 720);

        rectTransform.pivot = new Vector2(0, 0);

        if (mousePos.x + width > 1280)
        {
            rectTransform.pivot += new Vector2(1, 0);
        }
        if (mousePos.y + height > 720)
        {
            rectTransform.pivot += new Vector2(0, 1);
        }

        Debug.Log("Screen: " + Screen.width + ", " + Screen.height);
        Debug.Log("Mouse: " + mousePos);

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

                if (!currentEquipType.Solar)
                {
                    heatText += AppendScalingText(currentEquipType.HeatScaling, currentEquipment.powerStage);
                }

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

    public void HighlightBuildLocation()
    {
        if (dragging)
        {
            buildHighlight.gameObject.SetActive(true);
            Vector2Int gridPos = truck.GetMouseGridPosition();

            //Mouse is draging over a grid square
            if(gridPos.x >= 0)
            {
                buildHighlight.gameObject.SetActive(true);

                int sizeX = currentEquipType.Size.GridSize.x;
                int sizeY = currentEquipType.Size.GridSize.y;

                if(sizeX + gridPos.x > 4)
                {
                    gridPos.x--;
                }
                if(sizeY > 1)
                {
                    gridPos.y = Mathf.Max(gridPos.y - 1, 0);
                }

                buildHighlight.localPosition = new Vector3(gridPos.x * buildHighlightOffset.x, gridPos.y * buildHighlightOffset.y, 0);
                buildHighlight.sizeDelta = new Vector3(buildHighlightSize.x + (sizeX - 1) * buildTileSize.x, buildHighlightSize.y + (sizeY - 1) * buildTileSize.y, 0);
            }
            else
            {
                buildHighlight.gameObject.SetActive(false);
            }
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
        buildHighlight.gameObject.SetActive(false);

        truck.BuyEquipment(truck.GetMouseGridPosition(), currentEquipType);
    }
}
