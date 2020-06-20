using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    public bool debugDisplay;
    public int width, height, xOffset, yOffset, cellWidth, cellHeight;
    private GameObject[,] machines;

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

    void Start()
    {
        //machines = new GameObject[width,height];

        for(int col = 0; col < machines.Length; col++)
        {
            for(int row = 0; row < machines.Length; row++)
            {
                machines[col,row] = null;
            }
        }
    }

    public void InsertIntoSquare(int col, int row, GameObject g)
    {
        machines[col,row] = g;
        g.transform.position = new Vector3(col / width * gridPixelWidth + xOffset, row / height * gridPixelHeight + yOffset);
    }

    public GameObject GetSquare(int col, int row)
    {
        return machines[col,row];
    }

    public GameObject ScreenToGridCoords(float x, float y)
    {
        int col = (int)Mathf.Min((x - xOffset) / gridPixelWidth);
        int row = (int)Mathf.Min((y - yOffset) / gridPixelHeight);

        //If x/y are inside the grid
        if(col > 0 && col < width && row > 0 && row < height)
        {
            return machines[col, row];
        }
        return null;
    }

    private void OnDrawGizmos()
    {
        if (debugDisplay)
        {
            for (var col = 0; !(col > width); col++)
            {
                Debug.DrawLine(new Vector3(xOffset + col * cellWidth, yOffset), new Vector3(xOffset + col * cellWidth, yOffset + gridPixelHeight));
            }
            for (var row = 0; !(row > height); row++)
            {
                Debug.DrawLine(new Vector3(xOffset, yOffset + row * cellHeight), new Vector3(xOffset + gridPixelWidth, yOffset + row * cellHeight));
            }
        }
    }
}
