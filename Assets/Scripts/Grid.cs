using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform gridAnchor, gridFarCorner;
    public int width, height, cellWidth, cellHeight, horizontalGap, verticalGap;
    private Equipment[,] slots;
    public Equipment TEST, TEST2;
    private float spriteScale = 10;

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
        for(int c = col; c < col + equipment.type.Size.GridSize.x; c++)
        {
            for(int r = row; r < row + equipment.type.Size.GridSize.y; r++)
            {
                if(slots[c, r] != null)
                {
                    slots[c, r].type = null;

                    Destroy(slots[c, r].gameObject);
                }

                slots[c, r] = equipment;
            }
        }

        equipment.transform.parent = gridAnchor;
        equipment.transform.position = gridAnchor.TransformPoint(new Vector3(col * gridPixelWidth / width, row * gridPixelHeight / height));
        equipment.transform.localScale = new Vector3(spriteScale, spriteScale, spriteScale);
    }

    public Equipment GetEquipmentAt(Vector2Int point)
    {
        return slots[point.x, point.y];
    }

    public Equipment GetEquipmentAt(int col, int row)
    {
        return slots[col,row];
    }

    public List<Equipment> GetAllEquipment()
    {
        List<Equipment> allEquipment = new List<Equipment>();

        for (int i = 0; i < slots.GetLength(0); i++)
        {
            for (int j = 0; j < slots.GetLength(1); j++)
            {
                if (slots[i, j] && slots[i,j].type && !allEquipment.Contains(slots[i, j]))
                {
                    allEquipment.Add(slots[i, j]);
                }
            }
        }
        return allEquipment;
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

        //Remove space before grid and gaps between squares
        if (point.x < 0 || point.y < 0
         || point.x % cellWidth > cellWidth - horizontalGap || point.y % cellHeight > cellHeight - verticalGap)
        {
            return new Vector2Int(-1, -1);
        }

        int col = (int)(point.x / gridPixelWidth * width);
        int row = (int)(point.y / gridPixelHeight * height);

        if(col < 0 || col >= width
        || row < 0 || row >= height)
        {
            return new Vector2Int(-1, -1);
        }

        return new Vector2Int(col, row);
    }
}
