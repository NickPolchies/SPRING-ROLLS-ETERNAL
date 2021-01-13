﻿using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public float outsideTemperature, warmingFactor;
    public int day;
    public float dayLength;
    private float timeOfDay;
    public AnimationCurve weeklyTemperatureFlow;
    public float minRandomTemp, maxRandomTemp;
    public float powerWarning;

    public TextMeshProUGUI dayText, tempText, powerText, cashText;
    public Image insideThermometer;
    public Image outsideThermometer;
    public TruckController truck;
    public Sun sun;
    public ParticleSystem rain;

    private void Awake()
    {
        day = 1;
    }

    void Update()
    {
        timeOfDay += Time.deltaTime;

        if (timeOfDay > dayLength)
        {
            outsideTemperature += weeklyTemperatureFlow.Evaluate(day % 7) + Random.Range(minRandomTemp, maxRandomTemp);

            day++;
            timeOfDay -= dayLength;
        }

        dayText.text = "Day " + day;
        tempText.text = "Temp: " + truck.temperature.ToString("F1");
        powerText.text = "Power: " + truck.power.ToString("F0");

        cashText.text = "Cash: $" + truck.cash.ToString("F2");

        if (truck.power <= 0)
        {
            powerText.color = Color.red;
        }
        else if (truck.power <= powerWarning)
        {
            powerText.color = Color.yellow;
        }
        else
        {
            powerText.color = Color.green;
        }
        if (truck.temperature < truck.lowTemperature)
        {
            tempText.color = Color.blue;
        }
        else if (truck.temperature > truck.highTemperature)
        {
            tempText.color = Color.red;
        }
        else
        {
            tempText.color = Color.white;
        }

        truck.HeatTransfer(outsideTemperature);

        insideThermometer.fillAmount = (truck.temperature - truck.minTemperature) / (truck.maxTemperature - truck.minTemperature);
        outsideThermometer.fillAmount = (outsideTemperature - truck.minTemperature) / (truck.maxTemperature - truck.minTemperature);

        sun.setProgress(timeOfDay/dayLength);

        UpdateGlobalWarming();

        UpdateRain();
    }

    private void UpdateGlobalWarming()
    {
        float warming = truck.GetExternalHeatGeneration();
        outsideTemperature += warming * warmingFactor;
    }

    private void UpdateRain()
    {
        return; //TODO rain
        if (outsideTemperature < 25 && !rain.gameObject.activeInHierarchy)
        {
            rain.gameObject.SetActive(true);
        }
        else if (outsideTemperature > 25 && rain.gameObject.activeInHierarchy)
        {
            rain.gameObject.SetActive(false);
        }
    }
}
