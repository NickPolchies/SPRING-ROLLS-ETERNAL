using UnityEngine;
using UnityEngine.UI;

public class OutsideThermometer : MonoBehaviour
{
    public float temperature;
    public float coldMinTemperature, coldMaxTemperature;
    public float coolMinTemperature, coolMaxTemperature;
    public float warmMinTemperature, warmMaxTemperature;
    public float hotMinTemperature, hotMaxTemperature;

    public Image coldThermometerImage;
    public Image coolThermometerImage;
    public Image warmThermometerImage;
    public Image hotThermometerImage;

    void Update()
    {
        coldThermometerImage.fillAmount = (temperature - coldMinTemperature) / (coldMaxTemperature - coldMinTemperature);
        coolThermometerImage.fillAmount = (temperature - coolMinTemperature) / (coolMaxTemperature - coolMinTemperature);
        warmThermometerImage.fillAmount = (temperature - warmMinTemperature) / (warmMaxTemperature - warmMinTemperature);
        hotThermometerImage.fillAmount = (temperature - hotMinTemperature) / (hotMaxTemperature - hotMinTemperature);
    }
}
