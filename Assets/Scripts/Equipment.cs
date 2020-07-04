using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Equipment : MonoBehaviour, Clickable
{
    public float thermalRating;
    public int purchaseCost;
    public float upkeepCost;
    public float power;
    public int size;
    
    public bool roof;

    public bool powered;

    private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    public void CyclePower()
    {
        if (power >= 0 && upkeepCost < 0)
        {
            return;
        }

        powered = !powered;

        if (powered)
        {
            sprite.color = Color.white;
        }
        else
        {
            sprite.color = Color.red;
        }
    }

    public void Clicked(MouseButton button)
    {
        if(button == MouseButton.RightMouse)
        {
            CyclePower();
        }
    }
}
