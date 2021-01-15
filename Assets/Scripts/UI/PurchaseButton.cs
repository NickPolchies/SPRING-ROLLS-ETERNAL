using UnityEngine;
using UnityEngine.UI;

public class PurchaseButton : MonoBehaviour
{
    public EquipmentType equipmentType;
    private EquipmentPurchaseUI equipmentPurchaseUI;

    public void SetPurchaseUI(EquipmentPurchaseUI newEquipmentPurchaseUI)
    {
        equipmentPurchaseUI = newEquipmentPurchaseUI;
    }

    public void MouseEnter()
    {
        equipmentPurchaseUI.mouseUI.MouseEnter(equipmentType);
    }

    public void MouseExit()
    {
        equipmentPurchaseUI.mouseUI.MouseExit();
    }

    public void DragStart()
    {
        equipmentPurchaseUI.mouseUI.DragStart(GetComponent<Image>().sprite);
    }

    public void DragEnd()
    {
        equipmentPurchaseUI.mouseUI.DragStop();
    }
}
