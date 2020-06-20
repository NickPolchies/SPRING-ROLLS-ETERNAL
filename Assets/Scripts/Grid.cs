using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public Transform gridAnchor, gridFarCorner;
    public int width, height, cellWidth, cellHeight;
    private GameObject[,] machines;

    private void Start()
    {
        machines = new GameObject[width, height];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log(ScreenToGridCoords((Vector2)Input.mousePosition));
        }
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

    public void InsertIntoSquare(int col, int row, GameObject g)
    {
        machines[col,row] = g;
        g.transform.position = gridAnchor.TransformPoint(new Vector3(col * gridPixelWidth / width, row * gridPixelHeight / height));
    }

    public GameObject GetSquare(int col, int row)
    {
        return machines[col,row];
    }

    public Vector2 ScreenToGridCoords(Vector2 point)
    {
        return ScreenToGridCoords(point.x, point.y);
    }

    public Vector2 ScreenToGridCoords(float x, float y)
    {
        Vector3 point = new Vector3(x, y);
        point = Camera.main.ScreenToWorldPoint(point);
        point = gridAnchor.InverseTransformPoint(point);

        //Remove gaps between grid squares
        if (FilterX(point.x) || FilterY(point.y))
        {
            return new Vector2(-1, -1);
        }

        int col = (int)(point.x / gridPixelWidth * width);
        int row = (int)(point.y / gridPixelHeight * height);

        return new Vector2(col, row);
    }

    public GameObject GetObjectAtGridCoords(int col, int row)
    {
        //If x/y are inside the grid
        if (col > 0 && col < width && row > 0 && row < height)
        {
            return machines[col, row];
        }
        return null;
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
