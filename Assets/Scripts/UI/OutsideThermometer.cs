using UnityEngine;
using UnityEngine.UI;

public class OutsideThermometer : MonoBehaviour
{
    public float temperature;
    private float nextTemperature, previousTemperature;
    public float coldMinTemperature, coldMaxTemperature;
    public float coolMinTemperature, coolMaxTemperature;
    public float warmMinTemperature, warmMaxTemperature;
    public float hotMinTemperature, hotMaxTemperature;
    public float transitionTime;
    private float remainingTransitionTime;

    public Image coldThermometerImage;
    public Image coolThermometerImage;
    public Image warmThermometerImage;
    public Image hotThermometerImage;

    private void Start()
    {
        remainingTransitionTime = 0;
        previousTemperature = nextTemperature = temperature;
    }

    void Update()
    {
        if(remainingTransitionTime > 0)
        {
            float t = Easing.InOutExponentialClamped(1 - remainingTransitionTime / transitionTime);
            temperature = t * nextTemperature + (1 - t) * previousTemperature;
            remainingTransitionTime -= Time.deltaTime;
        }
        else if (remainingTransitionTime <= 0 && previousTemperature != temperature)
        {
            previousTemperature = temperature = nextTemperature;
            remainingTransitionTime = 0;
        }

        coldThermometerImage.fillAmount = (temperature - coldMinTemperature) / (coldMaxTemperature - coldMinTemperature);
        coolThermometerImage.fillAmount = (temperature - coolMinTemperature) / (coolMaxTemperature - coolMinTemperature);
        warmThermometerImage.fillAmount = (temperature - warmMinTemperature) / (warmMaxTemperature - warmMinTemperature);
        hotThermometerImage.fillAmount = (temperature - hotMinTemperature) / (hotMaxTemperature - hotMinTemperature);
    }

    public void IncreaseTemperature(float temperatureIncrease)
    {
        Debug.Log(temperatureIncrease);
        nextTemperature = temperature + temperatureIncrease;
        previousTemperature = temperature;
        remainingTransitionTime = transitionTime;
    }
}
