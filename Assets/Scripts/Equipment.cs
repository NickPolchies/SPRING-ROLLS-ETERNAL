using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Equipment : MonoBehaviour
{
    public float thermalRating;
    public int purchaseCost;
    public float upkeepCost;
    public float power;
    public int size;
    
    public bool roof;

    public bool powered;

    public void cyclePower()
    {
        powered = !powered;
    }
}
