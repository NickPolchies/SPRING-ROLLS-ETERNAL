using System;
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
    public float insulationRating = 10;
    public EnvironmentInfo environment;

    private void OnValidate()
    {
        if(startingEquipment.Length > 5)
        {
            Debug.LogError("ERR: Setting starting equipment to more than 5 will cause runtime errors");
        }
    }

    void Start()
    {
        temperature = environment.temperature;

        grid = GetComponent<Grid>();
        for(int i = 0; i < startingEquipment.Length; i++)
        {
            grid.AddEquipment(i, 0, startingEquipment[i]);
        }
    }

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

        temperature += (environment.temperature - temperature) / insulationRating * Time.deltaTime;

        environment.AddTime(Time.deltaTime);
    }
}
