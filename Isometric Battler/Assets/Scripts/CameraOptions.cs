using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOptions : MonoBehaviour
{
    Camera cam;
    [SerializeField]
    float sizeMax = 19;
    [SerializeField]
    float sizeMin = 5;


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
    }

}
