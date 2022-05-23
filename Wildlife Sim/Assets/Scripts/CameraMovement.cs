using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    [SerializeField]
    float speed;
    [SerializeField]
    float zoomSpeed;
    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 CameraMove = new Vector3(0, 0, 0);
        float CameraZoom = 0;
        if (Input.GetKey(KeyCode.UpArrow)) CameraMove.y += speed * Time.deltaTime; 
        if (Input.GetKey(KeyCode.DownArrow)) CameraMove.y -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.LeftArrow)) CameraMove.x -= speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.RightArrow)) CameraMove.x += speed * Time.deltaTime;
        if (Input.GetKey(KeyCode.S)) CameraZoom -= zoomSpeed * Time.deltaTime;
        if (Input.GetKey(KeyCode.W)) CameraZoom += zoomSpeed * Time.deltaTime;
        transform.position += CameraMove;
        cam.orthographicSize += CameraZoom;
    }
}
