﻿using System;
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

    private Grid grid;
    public Equipment[] startingEquipment = new Equipment[5];
    private float externalHeat;
    private MouseUI mouseUI; //TODO really hate how this works, maybe redo this

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
        mouseUI = FindObjectOfType<MouseUI>();

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

        float time = Time.time;

        foreach(Equipment e in equipment)
        {
            if (e)
            {
                RunEquipment(e, time);
            }
        }
    }

    public void BuyEquipment(Vector2Int point, Equipment e)
    {
        BuyEquipment(point.x, point.y, e);
    }

    public void BuyEquipment(int col, int row, Equipment equipPrefab)
    {
        if (equipPrefab.height == 2 && row == 1)
        {
            row = 0;
        }
        if (equipPrefab.width + col > grid.width)
        {
            col = grid.width - equipPrefab.width;
        }

        if (col < 0 || equipPrefab.roof && row < 2 || !equipPrefab.roof && row >= 2)
        {
            return;
        }

        List<Equipment> previousEquipment = GetPreviousEquipment(col, row, equipPrefab.width, equipPrefab.height);

        //float cashBack = 0;
        float powerBack = 0;

        foreach (Equipment e in previousEquipment)
        {
            //cashBack += e.purchaseCost/2;
            powerBack -= e.power;
        }

        //if (equipPrefab.purchaseCost <= cash + cashBack && power + powerBack + equipPrefab.power >= 0)
        if (cash - equipPrefab.purchaseCost >= 0 && power + powerBack + equipPrefab.power >= 0)
        {
            Equipment equipment = Instantiate(equipPrefab, new Vector3(0, 0, 0), Quaternion.identity);

            for (int i = equipPrefab.width - 1; i >= 0; i--)
            {
                for (int j = equipPrefab.height - 1; j >= 0; j--)
                {
                    if (grid.GetEquipmentAt(col + i, row + j))
                    {
                        //TODO will have to change because selling would need to check for power
                        Destroy(equipment);
                    }

                    grid.AddEquipment(col + i, row + j, equipment);
                }
            }

            InitializeEquipment(equipment);

            cash -= equipPrefab.purchaseCost;
        }
        return;
    }

    private void InitializeEquipment(Equipment e)
    {
        if (power + e.power < 0)
        {
            e.CyclePower();
        }
        else
        {
            power += e.power;
        }

        e.SetMouseUI(mouseUI);
    }

    private List<Equipment> GetPreviousEquipment(int col, int row, int width, int height)
    {
        List<Equipment> previousEquipment = new List<Equipment>();
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Equipment e = grid.GetEquipmentAt(col + i, row + j);

                if (e != null && !previousEquipment.Contains(e))
                {
                    previousEquipment.Add(grid.GetEquipmentAt(col + i, row + j));
                }
            }
        }

        return previousEquipment;
    }

    /*
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
    */

    public Vector2Int GetMouseGridPosition()
    {
        return grid.ScreenToGridCoords((Vector2)Input.mousePosition);
    }

    public void HeatTransfer(float outsideTemp)
    {
        temperature += (outsideTemp - temperature) / insulationRating * Time.deltaTime;
    }

    private void RunEquipment(Equipment e, float time)
    {
        Equipment.Stats stats = e.UpdateStats(power, time);

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
        List<Equipment> equipmentList = GetPreviousEquipment(0, 2, grid.width, 1);

        foreach(Equipment e in equipmentList)
        {
            t += e.thermalRating;
        }

        return t;
    }

    public Equipment GetEquipmentAtMouse()
    {
        Vector2Int mouseGridCoords = GetMouseGridPosition();

        if(mouseGridCoords.x > -1)
        {
            return grid.GetEquipmentAt(GetMouseGridPosition());
        }
        return null;
    }
}
