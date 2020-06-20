using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Equipment[] equipmentList;
    public Button[] equipmentButtons;
    public TruckController truck;
    public EnvironmentInfo environmentInfo;
    
    [Header("Menu Text")]
    public TextMeshProUGUI dayText;

    private Equipment buying;
    private bool mouseDown;

    void Start()
    {
        buying = null;
        for (int i = 1; i < equipmentButtons.Length; i++)
        {
            TextMeshProUGUI buttonText = equipmentButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            buttonText.text = "COST\n" + equipmentList[i].upkeepCost;

            Equipment e = equipmentList[i];

            equipmentButtons[i].onClick.AddListener(() => { Debug.Log("FFFFF"); BuyItem(e); });
//            equipmentButtons[i]
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
        }
        if (Input.GetMouseButtonUp(0) && buying != null)
        {
            Debug.Log("TEST");
            truck.buyEquipment(truck.getMouseGridPosition(), buying);
        }
        //dayText.text = "Day " + environmentInfo.day;
    }

    public void BuyItem(Equipment e)
    {
        buying = e;
        truck.buyEquipment(truck.getMouseGridPosition(), buying);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        mouseDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
