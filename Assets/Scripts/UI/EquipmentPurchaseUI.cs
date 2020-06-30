using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EquipmentPurchaseUI : MonoBehaviour
{
    public TruckController truck;
    public Equipment[] equipmentList;
    public Button[] equipmentButtons;

    private Equipment buying;

    void Start()
    {
        buying = null;

        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "$" + equipmentList[i].purchaseCost;

            Equipment e = equipmentList[i];

            equipmentButtons[i].onClick.AddListener(() => { Debug.Log("FFF"); BuyItem(e); });
        }

    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && buying != null)
        {
            truck.buyEquipment(truck.GetMouseGridPosition(), buying);
        }
    }

    public void BuyItem(Equipment e)
    {
        buying = e;
    }
}
