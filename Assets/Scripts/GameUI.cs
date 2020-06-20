using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Equipment[] equipmentList;
    public Button[] equipmentButtons;
    public Equipment buying;
    public TruckController truck;
    public EnvironmentInfo environmentInfo;
    public TextMeshProUGUI dayText;

    void Start()
    {
        buying = null;
        for(int i = 1; i < equipmentButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "COST\n" + equipmentList[i].upkeepCost;
            equipmentButtons[i].onClick.AddListener(() => BuyItem(equipmentList[i]));
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0) && buying != null)
        {
            Debug.Log("TEST");
            truck.buyEquipment(truck.getMouseGridPosition(), buying);
        }
        //dayText.text = "Day " + environmentInfo.day;
    }

    public void BuyItem(Equipment e)
    {
        Debug.Log("TEAGAER");
        buying = e;
    }
}
