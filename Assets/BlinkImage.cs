using UnityEngine;
using UnityEngine.UI;

public class BlinkImage : MonoBehaviour
{
    private Image image;
    private Color color;
    private bool asc;
    public float blinkSpeed;

    void Start()
    {
        image = GetComponent<Image>();
        color = new Color(1, 1, 1, 1);
        asc = false;
    }

    void Update()
    {
        color.a += (asc ? Time.deltaTime : -Time.deltaTime) * blinkSpeed;

        if(color.a > 1)
        {
            color.a = 1;
            asc = false;
        }
        else if(color.a < 0)
        {
            color.a = 0;
            asc = true;
        }

        image.color = color;
    }
}
