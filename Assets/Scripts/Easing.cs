public static class Easing
{
    public static float InOutExponentialClamped(float t)
    {
        if(t <= 0)
        {
            return 0;
        }
        else if(t >= 1)
        {
            return 1;
        }
        else if(t < 0.5)
        {
            return UnityEngine.Mathf.Pow(2, 20 * t - 10)/2;
            //return pow(2, 20 * x - 10) / 2
        }
        else
        {
            return (2 - UnityEngine.Mathf.Pow(2, -20 * t + 10)) / 2;
            //return (2 - pow(2, -20 * x + 10)) / 2
        }
    }
}
