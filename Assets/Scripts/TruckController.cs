using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckController : MonoBehaviour
{
    public int cash;
    public float watts, temperature;
    private Grid grid;
    public Equipment[] startingEquipment = new Equipment[5];
    public TextMeshProUGUI TEMP;
    public float outsideTemperature;

    private void OnValidate()
    {
        if(startingEquipment.Length > 5)
        {
            Debug.LogError("ERR: Setting starting equipment to more than 5 will cause runtime errors");
        }
    }

    void Start()
    {
        grid = GetComponent<Grid>();
        for(int i = 0; i < startingEquipment.Length; i++)
        {
            grid.AddEquipment(i, 0, startingEquipment[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        TEMP.text = temperature.ToString();

        List<Equipment> equipment = grid.GetAllEquipment();

        foreach(Equipment e in equipment)
        {
            if (e)
            {
                temperature += e.ThermalRating * Time.deltaTime;
            }
        }

        temperature += (outsideTemperature - temperature) /10 * Time.deltaTime;
    }
}
