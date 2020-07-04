using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InputController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown((int)MouseButton.RightMouse))
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            Debug.Log("rClick at: " + mousePos2D);
            Vector2 dir = Vector2.zero;


            //RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector3.up,1,LayerMask.NameToLayer("Equipment"));
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, dir);

            Debug.DrawRay(mousePos2D, dir, Color.red, 10, false);

            if(hit.collider != null)
            {
                Debug.Log("Hit a " + hit.collider.gameObject.name);
                Clickable e = hit.collider.GetComponent<Clickable>();
                if (e != null)
                {
                    Debug.Log("Hit equip");
                    e.Clicked(MouseButton.RightMouse);
                }
            }
        }
    }
}
