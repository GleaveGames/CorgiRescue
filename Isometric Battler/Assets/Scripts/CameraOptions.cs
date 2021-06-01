using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptions : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    float sizeMax = 25;
    [SerializeField]
    float sizeMin = 5;
    Vector3 dragOrigin;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    private void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            if (cam.orthographicSize > sizeMin)
            {
                cam.orthographicSize--;
            }
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            if (cam.orthographicSize < sizeMax)
            {
                cam.orthographicSize++;
            }
        }
        PanCamera();
    }
    void PanCamera() 
    {
        if (Input.GetMouseButtonDown(1)) 
        {
            dragOrigin = cam.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(1)) 
        {
            Vector3 difference = dragOrigin - cam.ScreenToWorldPoint(Input.mousePosition);

            cam.transform.position += difference;
        
        }

    }
}
