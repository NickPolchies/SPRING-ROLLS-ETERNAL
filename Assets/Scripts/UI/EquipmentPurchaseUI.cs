using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPurchaseUI : MonoBehaviour
{
    public TruckController truck;

    private Button[] equipmentButtons;
    private Equipment buying;

    void Start()
    {
        equipmentButtons = GetComponentsInChildren<Button>();
        buying = null;

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            Equipment equipment = equipmentButtons[i].GetComponent<EquipmentButton>().equipment;
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();

            buttonText.text = "$" + equipment.purchaseCost;

            equipmentButtons[i].onClick.AddListener(() => { BuyItem(equipment); });
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

    public void BuyItem(Equipment e)
    {
        buying = e;
    }
}
