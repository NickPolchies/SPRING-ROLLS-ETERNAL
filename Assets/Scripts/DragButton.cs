using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DragButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Equipment equipment;

    public UnityEvent onDragClick;

    private bool pointerDown;

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = true;
        Debug.Log("HoldingDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Debug.Log("Released");
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (pointerDown)
        {

        }
    }
}
