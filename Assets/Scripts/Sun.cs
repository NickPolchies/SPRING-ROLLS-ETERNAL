using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    private float width;
    private float eastPoint, westPoint;

    void Start()
    {
        width = GetComponent<SpriteRenderer>().bounds.size.x;
        eastPoint = Camera.main.ScreenToWorldPoint(new Vector3(Camera.main.pixelWidth, 0, 0)).x + width/2;
        westPoint = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0)).x - width/2;
    }

    public void setProgress(float t)
    {
        transform.position = new Vector3(t * (westPoint - eastPoint) + eastPoint, transform.position.y);
    }
}
