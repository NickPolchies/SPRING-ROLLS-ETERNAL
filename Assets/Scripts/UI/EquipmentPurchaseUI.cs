using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPurchaseUI : MonoBehaviour
{
    public TruckController truck;
    public MouseUI mouseUI;

    private PurchaseButton[] equipmentButtons;
    private EquipmentType buying;

    void Start()
    {
        equipmentButtons = GetComponentsInChildren<PurchaseButton>();
        buying = null;

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            equipmentButtons[i].SetPurchaseUI(this);
            EquipmentType equipment = equipmentButtons[i].equipmentType;
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = "$" + equipment.Cost;

//            equipmentButtons[i].onClick.AddListener(() => { BuyItem(equipment); });
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && buying != null)
        {
            truck.BuyEquipment(truck.GetMouseGridPosition(), buying);
            buying = null;
        }
    }

    public void BuyItem(EquipmentType e)
    {
        buying = e;
    }
}
