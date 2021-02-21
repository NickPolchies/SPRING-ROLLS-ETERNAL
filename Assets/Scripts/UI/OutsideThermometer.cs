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
    [SerializeField] private Animator glow;

    private void Start()
    {
        remainingTransitionTime = 0;
        previousTemperature = nextTemperature = temperature;
//        glow.keepAnimatorControllerStateOnDisable = false;
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

        if(nextTemperature > temperature)
        {
//            glow.enabled = true;
            glow.SetFloat("RedBarFill", 1);
        }
        else if(nextTemperature < temperature)
        {
//            glow.enabled = true;
            glow.SetFloat("RedBarFill", 0);
        }
        else
        {
//            glow.enabled = false;
            glow.SetFloat("RedBarFill", 0.5f);
        }

        coldThermometerImage.fillAmount = (temperature - coldMinTemperature) / (coldMaxTemperature - coldMinTemperature);
        coolThermometerImage.fillAmount = (temperature - coolMinTemperature) / (coolMaxTemperature - coolMinTemperature);
        warmThermometerImage.fillAmount = (temperature - warmMinTemperature) / (warmMaxTemperature - warmMinTemperature);
        hotThermometerImage.fillAmount = (temperature - hotMinTemperature) / (hotMaxTemperature - hotMinTemperature);
    }

    public void IncreaseTemperature(float temperatureIncrease)
    {
        nextTemperature = temperature + temperatureIncrease;
        previousTemperature = temperature;
        remainingTransitionTime = transitionTime;
    }
}
