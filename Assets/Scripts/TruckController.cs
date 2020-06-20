using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckController : MonoBehaviour
{
    public float cash, power, temperature;
    public float truckGeneratorPower;
    private Grid grid;
    public Equipment[] startingEquipment = new Equipment[5];
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
        List<Equipment> equipment = grid.GetAllEquipment();

        power = truckGeneratorPower;

        foreach(Equipment e in equipment)
        {
            if (e)
            {
                temperature += e.thermalRating * Time.deltaTime;
                cash += e.upkeepCost * Time.deltaTime;
                power += e.power;
            }
        }

        temperature += (environment.temperature - temperature) / insulationRating * Time.deltaTime;

        environment.AddTime(Time.deltaTime);
    }

    public void buyEquipment(Vector2Int point, Equipment e)
    {
        buyEquipment(point.x, point.y, e);
    }

    public void buyEquipment(int col, int row, Equipment e)
    {
        if(e.purchaseCost < cash)
        {
            if(grid.GetEquipmentAt(col, row) != null)
            {
                cash += grid.GetEquipmentAt(col, row).purchaseCost/2;
            }

            grid.AddEquipment(col, row, e);
        }
    }

    public Vector2Int getMouseGridPosition()
    {
        return grid.ScreenToGridCoords((Vector2)Input.mousePosition);
    }
}
