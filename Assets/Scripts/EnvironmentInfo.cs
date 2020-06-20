using TMPro;
using UnityEngine;

public class EnvironmentInfo : MonoBehaviour
{
    public float temperature;
    public int day;
    public float dayLength;
    private float timeOfDay;
    public AnimationCurve weeklyTemperatureFlow;

    public void AddTime(float time)
    {
        timeOfDay += time;

        if(timeOfDay > dayLength)
        {
            day++;
            timeOfDay -= dayLength;

            temperature += weeklyTemperatureFlow.Evaluate(day / 7);
            temperature += Random.Range(-3f, 3f);
        }
    }
}
