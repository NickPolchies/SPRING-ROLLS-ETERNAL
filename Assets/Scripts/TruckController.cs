using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckController : MonoBehaviour
{
    public float cash, power, temperature;
    public float warningHighTemp, maxTemp, warningLowTemp, minTemp;
    public float truckGeneratorPower;
    private Grid grid;
    public Equipment[] startingEquipment = new Equipment[5];
    public float outsideTemperature;
    public float insulationRating = 10;
    public float lifetimeCash = 0;

    public struct TruckStats
    {
        float cash, power, temperature;
    }

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
            Equipment equipment = Instantiate(startingEquipment[i], new Vector3(0, 0, 0), Quaternion.identity);
            grid.AddEquipment(i, 0, equipment);
            equipment.powered = true;
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
                temperature += e.thermalRating * 10 * Time.deltaTime;
                cash -= e.upkeepCost * 10 * Time.deltaTime;
                lifetimeCash += e.upkeepCost < 0 ? e.upkeepCost : 0;

                if (e.powered)
                {
                    power += e.power;
                }
            }
        }

        if(temperature > maxTemp)
        {
            GameOverHighTemp();
        }
        else if(temperature < minTemp)
        {
            GameOverLowTemp();
        }
    }

    public void buyEquipment(Vector2Int point, Equipment e)
    {
        buyEquipment(point.x, point.y, e);
    }

    public void buyEquipment(int col, int row, Equipment equipPrefab)
    {
        if (col < 0 || equipPrefab.roof && row < 2 || !equipPrefab.roof && row >= 2 || equipPrefab.size + col > grid.width)
        {
            return;
        }

        if(equipPrefab.purchaseCost < cash)
        {
            Equipment equipment = Instantiate(equipPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            for (int i = equipPrefab.size - 1; i >= 0; i--)
            {
                Debug.Log(col + i + ", " + row);

                //Refund previous equipment in slot/s
                if (grid.GetEquipmentAt(col + i, row))
                {
                    cash += grid.GetEquipmentAt(col + i, row).purchaseCost / 2;
                }
                grid.AddEquipment(col + i, row, equipment);
            }

            equipment.powered = true;
            cash -= equipPrefab.purchaseCost;
        }
        return;
    }

    public Vector2Int GetMouseGridPosition()
    {
        return grid.ScreenToGridCoords((Vector2)Input.mousePosition);
    }

    private void GameOverHighTemp()
    {

    }

    private void GameOverLowTemp()
    {

    }

    public void HeatTransfer(float outsideTemp)
    {
        temperature += (outsideTemp - temperature) / insulationRating * Time.deltaTime;
    }
}
