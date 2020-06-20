using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public int cash;
    public float watts, temperature;
    private Grid grid;
    public GameObject[] startingEquipment = new GameObject[5];

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
            grid.AddEquipment(i, 0, startingEquipment[i]);
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
