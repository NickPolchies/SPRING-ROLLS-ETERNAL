using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StatusUI : MonoBehaviour
{
    public float warmingFactor;
    public int day;
    public float dayLength;
    private float timeOfDay;
    public AnimationCurve weeklyTemperatureFlow;
    public float minRandomTemp, maxRandomTemp;
    public float powerWarning;
    public GameObject RainAudio;
    public float tempDayFactor;

    public OutsideThermometer outsideThermometer;
    public TruckController truck;
    public Sun sun;

    public TextMeshProUGUI dayText, tempText, intempText, powerText, cashText;
    public Image insideThermometer;
    public ParticleSystem rain;
    private Animator animator;

    public List<float> dailyOutsideTemperature { get; private set; }
    public List<float> dailyTotalCash { get; private set; }

    private void Awake()
    {
        day = 1;

        dailyOutsideTemperature = new List<float>();
        dailyTotalCash = new List<float>();

        dailyOutsideTemperature.Add(outsideThermometer.temperature);
        dailyTotalCash.Add(truck.startingCash);

        animator = GetComponentInChildren<Animator>();
    }

    void Update()
    {
        timeOfDay += Time.deltaTime;

        if (timeOfDay > dayLength)
        {
            day++;

            outsideThermometer.temperature += (weeklyTemperatureFlow.Evaluate(day % 5) + Random.Range(minRandomTemp, maxRandomTemp)) * (1f + tempDayFactor * day);

            if (day % 5 == 0)
            {
                rain.gameObject.SetActive(true);
                RainAudio.SetActive(true);
            }
            else
            {
                rain.gameObject.SetActive(false);
                RainAudio.SetActive(false);

            }

            dailyOutsideTemperature.Add(outsideThermometer.temperature);
            dailyTotalCash.Add(truck.lifetimeCash);

            timeOfDay -= dayLength;
        }

        truck.HeatTransfer(outsideThermometer.temperature);

        insideThermometer.fillAmount = (truck.temperature - truck.minTemperature) / (truck.maxTemperature - truck.minTemperature);
        animator.SetFloat("RedBarFill", insideThermometer.fillAmount);

        sun.setProgress(timeOfDay / dayLength);

        UpdateText();
    }

    private void UpdateText()
    {
        dayText.text = "Day " + day;
        tempText.text = outsideThermometer.temperature.ToString("F1") + "°";
        intempText.text = truck.temperature.ToString("F1") + "°";
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
            intempText.color = Color.cyan;
        }
        else if (truck.temperature > truck.highTemperature)
        {
            intempText.color = Color.red;
        }
        else
        {
            intempText.color = Color.white;
        }
    }
}
