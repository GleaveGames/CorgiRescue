using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSprite : MonoBehaviour
{
    float dist;
    bool dragging = false;
    Vector3 offset;
    Transform toDrag;
    GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);

            if (targetObject != null) Debug.Log(targetObject.name);

            if (targetObject && targetObject == this.gameObject.GetComponent<Collider2D>())
            {
                Debug.Log("picked up sprite");
                gc.draggingObj = this.gameObject;
            }
        }
    }
}
