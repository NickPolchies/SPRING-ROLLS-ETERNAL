﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public TruckController truck;

    public Canvas statusUI;
    public Canvas purchaseUI;
    public Canvas gameOverUI;

    void Start()
    {
        statusUI.gameObject.SetActive(true);
        purchaseUI.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);
    }

    void Update()
    {
        if (truck.temperature > truck.maxTemperature || truck.temperature < truck.minTemperature)
        {
            statusUI.gameObject.SetActive(false);
            purchaseUI.gameObject.SetActive(false);
            gameOverUI.gameObject.SetActive(true);
        }
    }
}
