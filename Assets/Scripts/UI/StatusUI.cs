using System.Collections;
using System.Collections.Generic;
using System.Security;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusUI : MonoBehaviour
{
    public float outdoorTemp;
    public int day;
    public float dayLength;
    private float timeOfDay;
    public AnimationCurve weeklyTemperatureFlow;

    public TextMeshProUGUI dayText, tempText, powerText, cashText;
    public Image insideThermometer;
    public Image outsideThermometer;
    public TruckController truck;
    public Sun sun;
    public ParticleSystem rain;

    void Update()
    {
        timeOfDay += Time.deltaTime;

        if (timeOfDay > dayLength)
        {
            outdoorTemp += weeklyTemperatureFlow.Evaluate(day % 7);
            outdoorTemp += Random.Range(-3f, 3f);

            //Increment afterwards because day starts at 1 rather than 0
            day++;
            timeOfDay -= dayLength;
        }

        dayText.text = "Day " + day;
        tempText.text = "Temp " + truck.temperature.ToString("F1");
        powerText.text = "Power " + truck.power.ToString("F0");

        /*//TODO
        cashUpdateTimer += Time.deltaTime;
        if (cashUpdateTimer >= 1f)
        {
            cashUpdateTimer -= 1f;
            cashText.text = "Cash " + truck.cash.ToString("F2");
        }
        */

        cashText.text = "Cash " + truck.cash.ToString("F2");

        if (truck.power < 0)
        {
            powerText.color = Color.red;
        }
        else
        {
            powerText.color = Color.white;
        }
        if (truck.temperature < truck.warningLowTemp)
        {
            tempText.color = Color.blue;
        }
        else if (truck.temperature > truck.warningHighTemp)
        {
            tempText.color = Color.red;
        }
        else
        {
            tempText.color = Color.white;
        }

        truck.HeatTransfer(outdoorTemp);

        insideThermometer.fillAmount = (truck.temperature - truck.minTemp) / (truck.maxTemp - truck.minTemp);
        outsideThermometer.fillAmount = (outdoorTemp - truck.minTemp) / (truck.maxTemp - truck.minTemp);

        sun.setProgress(timeOfDay/dayLength);

        UpdateRain();
    }

    private void UpdateRain()
    {
        return; //TODO rain
        if (outdoorTemp < 25 && !rain.gameObject.activeInHierarchy)
        {
            rain.gameObject.SetActive(true);
        }
        else if (outdoorTemp > 25 && rain.gameObject.activeInHierarchy)
        {
            rain.gameObject.SetActive(false);
        }
    }
}
