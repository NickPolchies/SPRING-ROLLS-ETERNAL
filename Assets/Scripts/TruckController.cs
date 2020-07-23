using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TruckController : MonoBehaviour
{
    public float startingCash, truckGeneratorPower, startingTemperature;
    public float cash, power, temperature;
    public float highTemperature, maxTemperature, lowTemperature, minTemperature;
    public float lifetimeCash;

    public float insulationRating = 10;
    //public float outsideTemperature;

    private Grid grid;
    public Equipment[] startingEquipment = new Equipment[5];
    private float externalHeat;

    private void OnValidate()
    {
        if(startingEquipment.Length > 4)
        {
            Debug.LogError("ERR: Setting starting equipment to more than 5 will cause runtime errors");
        }
    }

    void Start()
    {
        grid = GetComponent<Grid>();

        power = 0;
        cash = 999999;
        externalHeat = 0;

        power += truckGeneratorPower;

        for (int i = 0; i < startingEquipment.Length; i++)
        {
            BuyEquipment(i, 0, startingEquipment[i]);

            /*
            Equipment equipment = Instantiate(startingEquipment[i], new Vector3(0, 0, 0), Quaternion.identity);
            grid.AddEquipment(i, 0, equipment);
            equipment.powered = true;
            */
        }

        cash = startingCash;
        temperature = startingTemperature;

        lifetimeCash = 0;
    }

    void Update()
    {
        List<Equipment> equipment = grid.GetAllEquipment();

        //power = truckGeneratorPower;

        foreach(Equipment e in equipment)
        {
            if (e)
            {
                RunEquipment(e);
            }
        }
    }

    public void BuyEquipment(Vector2Int point, Equipment e)
    {
        BuyEquipment(point.x, point.y, e);
    }

    //validate equipment placement
    //get existing equipment
    //get cost/power changes
    //validate power/cost
    //remove existing equipment
    //place new equipment
    //change power/cost
    public void BuyEquipment(int col, int row, Equipment equipPrefab)
    {
        if (col < 0 || equipPrefab.roof && row < 2 || !equipPrefab.roof && row >= 2 || equipPrefab.size + col > grid.width)
        {
            return;
        }

        List<Equipment> previousEquipment = GetPreviousEquipment(col, row, equipPrefab.size);

        float cashBack = 0, powerBack = 0;
        foreach (Equipment e in previousEquipment)
        {
            cashBack += e.purchaseCost;
            powerBack -= e.power;
        }

        if (equipPrefab.purchaseCost <= cash + cashBack && power + powerBack + equipPrefab.power >= 0)
        {
            Equipment equipment = Instantiate(equipPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            for (int i = equipPrefab.size - 1; i >= 0; i--)
            {
                //Refund previous equipment in slot/s
                if (grid.GetEquipmentAt(col + i, row))
                {
                    //TODO will have to change because selling would need to check for power
                    SellEquipment(col + i, row);
                }

                grid.AddEquipment(col + i, row, equipment);
            }

            if (power + equipment.power < 0)
            {
                equipment.CyclePower();
            }
            else
            {
                power += equipment.power;
            }

            cash -= equipPrefab.purchaseCost;
        }
        return;
    }

    private List<Equipment> GetPreviousEquipment(int col, int row, int sizeX)
    {
        List<Equipment> previousEquipment = new List<Equipment>();
        for (int i = sizeX - 1; i >= 0; i--)
        {
            Equipment e = grid.GetEquipmentAt(col + i, row);

            if (e != null && !previousEquipment.Contains(e))
            {
                previousEquipment.Add(grid.GetEquipmentAt(col + i, row));
            }
        }

        return previousEquipment;
    }

    public void SellEquipment(Vector2Int point)
    {
        SellEquipment(point.x, point.y);
    }

    public void SellEquipment(int col, int row)
    {
        Equipment equipment = grid.GetEquipmentAt(col, row);

        cash += equipment.purchaseCost / 2;
        power -= equipment.powered ? equipment.power : 0;

        Destroy(equipment);
    }

    public Vector2Int GetMouseGridPosition()
    {
        return grid.ScreenToGridCoords((Vector2)Input.mousePosition);
    }

    public void HeatTransfer(float outsideTemp)
    {
        temperature += (outsideTemp - temperature) / insulationRating * Time.deltaTime;
    }

    private void RunEquipment(Equipment e)
    {
        Equipment.Stats stats = e.UpdateStats(power);

        if (!e.roof)
        {
            temperature += stats.heat * Equipment.tickLength;
        }

        power += stats.power;
        cash += stats.cash * Equipment.tickLength;
        lifetimeCash += stats.cash > 0 ? stats.cash : 0;
    }

    public float GetExternalHeatGeneration()
    {
        float t = 0;
        List<Equipment> equipmentList = GetPreviousEquipment(0, 2, grid.width);

        foreach(Equipment e in equipmentList)
        {
            t += e.thermalRating;
        }

        return t;
    }

    public Equipment GetEquipmentStatsAtMouse()
    {
        Vector2Int mouseGridCoords = GetMouseGridPosition();

        if(mouseGridCoords.x > -1)
        {
            return grid.GetEquipmentAt(GetMouseGridPosition());
        }
        return null;
    }
}
