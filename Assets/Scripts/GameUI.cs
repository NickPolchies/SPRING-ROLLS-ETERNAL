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
    private float cashUpdateTimer;

    void Start()
    {
        buying = null;

        cashUpdateTimer = 0f;
        cashText.text = "Cash " + truck.cash.ToString("F2");

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
        UpdateUI();
        
        if (Input.GetMouseButtonDown(0) && buying != null)
        {
            truck.buyEquipment(truck.GetMouseGridPosition(), buying);
            cashText.text = "Cash " + truck.cash.ToString("F2");
        }
    }

    private void UpdateUI()
    {
        dayText.text = "Day " + environmentInfo.day;
        tempText.text = "Temp " + truck.temperature.ToString("F1");
        powerText.text = "Power " + truck.power.ToString("F0");

        cashUpdateTimer += Time.deltaTime;
        if (cashUpdateTimer >= 1f)
        {
            cashUpdateTimer -= 1f;
            cashText.text = "Cash " + truck.cash.ToString("F2");
        }

        if (truck.power < 0)
        {
            powerText.color = Color.red;
        }
        else
        {
            powerText.color = Color.white;
        }
        if (truck.temperature < truck.warningLowTemp)
        {
            tempText.color = Color.blue;
        }
        else if (truck.temperature > truck.warningHighTemp)
        {
            tempText.color = Color.red;
        }
        else
        {
            tempText.color = Color.white;
        }
    }

    public void BuyItem(Equipment e)
    {
        buying = e;
        //truck.buyEquipment(truck.getMouseGridPosition(), buying);
    }
}
