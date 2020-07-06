﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform gridAnchor, gridFarCorner;
    public int width, height, cellWidth, cellHeight;
    private Equipment[,] slots;
    public Equipment TEST, TEST2;

    private void Awake()
    {
        slots = new Equipment[width, height];
    }

    public int gridPixelWidth
    {
        get
        {
            return cellWidth * width;
        }
    }

    public int gridPixelHeight
    {
        get
        {
            return cellHeight * height;
        }
    }
    public void AddEquipment(Vector2Int point, Equipment equipment)
    {
        AddEquipment(point.x, point.y, equipment);
    }

    public void AddEquipment(int col, int row, Equipment equipment)
    {
        if(slots[col, row] != null)
        {
            Destroy(slots[col, row].gameObject);
        }

        slots[col, row] = equipment;

        equipment.transform.parent = gridAnchor;
        equipment.transform.position = gridAnchor.TransformPoint(new Vector3(col * gridPixelWidth / width, row * gridPixelHeight / height));
    }

    public Equipment GetEquipmentAt(Vector2Int point)
    {
        return slots[point.x, point.y];
    }

    public Equipment GetEquipmentAt(int col, int row)
    {
        return slots[col,row];
    }

    public Vector2Int ScreenToGridCoords(Vector2 point)
    {
        return ScreenToGridCoords(point.x, point.y);
    }

    public Vector2Int ScreenToGridCoords(float x, float y)
    {
        Vector3 point = new Vector3(x, y);
        point = Camera.main.ScreenToWorldPoint(point);
        point = gridAnchor.InverseTransformPoint(point);

        //Remove gaps between grid squares
        if (FilterX(point.x) || FilterY(point.y))
        {
            return new Vector2Int(-1, -1);
        }

        int col = (int)(point.x / gridPixelWidth * width);
        int row = (int)(point.y / gridPixelHeight * height);

        return new Vector2Int(col, row);
    }

    public Equipment GetObjectAtGridCoords(int col, int row)
    {
        //If x/y are inside the grid
        if (col > 0 && col < width && row > 0 && row < height)
        {
            return slots[col, row];
        }
        return null;
    }

    public List<Equipment> GetAllEquipment()
    {
        List<Equipment> allEquipment = new List<Equipment>();

        for(int i = 0; i < slots.GetLength(0); i++)
        {
            for(int j = 0; j < slots.GetLength(1); j++)
            {
                allEquipment.Add(slots[i, j]);
            }
        }
        return allEquipment;
    }

    //Hack to fix grid
    private bool FilterX(float x)
    {
        if(x < 0
            || x >= 100
            || x < 81 && x > 80
            || x < 61 && x > 60
            || x < 41 && x > 40
            || x < 21 && x > 20)
        {
            return true;
        }
        return false;
    }

    //Another hack to fix the grid
    private bool FilterY(float y)
    {
        if(y < 0
           || y >= 66 
           || y < 49 && y > 42
           || y < 25 && y > 18)
        {
            return true;
        }
        return false;
    }
}
