using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector3.up,1,LayerMask.NameToLayer("Equipment"));
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if(hit.collider != null)
            {
                Clickable e = hit.collider.GetComponent<Clickable>();
                if (e != null)
                {
                    e.Clicked(MouseButton.RightMouse);
                }
            }
        }
    }
}
