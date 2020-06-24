using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Equipment[] equipmentList;
    public Button[] equipmentButtons;
    public TruckController truck;
    public EnvironmentInfo environmentInfo;
    
    [Header("Menu Text")]
    public TextMeshProUGUI dayText;
    public TextMeshProUGUI cashText;
    public TextMeshProUGUI tempText;
    public TextMeshProUGUI powerText;

    private Equipment buying;

    void Start()
    {
        buying = null;
        for (int i = 0; i < equipmentButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "COST\n" + equipmentList[i].purchaseCost;

            Equipment e = equipmentList[i];

            equipmentButtons[i].onClick.AddListener(() => { Debug.Log("FFF"); BuyItem(e); });
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && buying != null)
        {
            truck.buyEquipment(truck.getMouseGridPosition(), buying);
        }
        dayText.text = "Day " + environmentInfo.day;
        cashText.text = "Cash " + truck.cash.ToString("F2");
        tempText.text = "Temp " + truck.temperature.ToString("F1");
        powerText.text = "Power " + truck.power.ToString("F0");
    }

    public void BuyItem(Equipment e)
    {
        buying = e;
        //truck.buyEquipment(truck.getMouseGridPosition(), buying);
    }
}
