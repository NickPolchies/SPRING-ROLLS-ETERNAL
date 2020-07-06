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

        power = 0;
        cash = 999999;

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

    public void BuyEquipment(int col, int row, Equipment equipPrefab)
    {
        if (col < 0 || equipPrefab.roof && row < 2 || !equipPrefab.roof && row >= 2 || equipPrefab.size + col > grid.width)
        {
            return;
        }

        if(equipPrefab.purchaseCost <= cash)
        {
            Equipment equipment = Instantiate(equipPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            for (int i = equipPrefab.size - 1; i >= 0; i--)
            {
                //Refund previous equipment in slot/s
                if (grid.GetEquipmentAt(col + i, row))
                {
                    cash += grid.GetEquipmentAt(col + i, row).purchaseCost / 2;
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

        power += stats.power;
        //temperature += e.thermalRating * 10 * e.tickLength;
        temperature += stats.heat * Equipment.tickLength;
        cash += stats.cash * Equipment.tickLength;
        lifetimeCash += stats.cash > 0 ? stats.cash : 0;
    }
}
