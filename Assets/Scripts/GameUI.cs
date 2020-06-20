using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Equipment[] equipmentList;
    public Button[] equipmentButtons;

    void Start()
    {
        for(int i = 1; i < equipmentButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "COST\n" + equipmentList[i].upkeepCost;
        }
    }

    void Update()
    {
        
    }

    void buyItem()
    {

    }
}
