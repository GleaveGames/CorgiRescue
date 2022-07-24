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
    public bool onPlayer;
    Transform playerTransform;
    [SerializeField]
    float onPlayerZoom;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        cam = GetComponent<Camera>();
    }

    void FixedUpdate()
    {
        Vector3 CameraMove = new Vector3(0, 0, 0);
        float CameraZoom = 0;
        if (!onPlayer)
        {
            if (Input.GetKey(KeyCode.W)) CameraMove.y += speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.S)) CameraMove.y -= speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.A)) CameraMove.x -= speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.D)) CameraMove.x += speed * Time.deltaTime;
            if (Input.GetKey(KeyCode.Q)) CameraZoom -= zoomSpeed * Time.deltaTime;
            if (Input.GetKey(KeyCode.E)) CameraZoom += zoomSpeed * Time.deltaTime;
        }
        else
        {
            CameraMove = (playerTransform.position - transform.position).normalized * speed * Time.deltaTime;
            CameraMove.z = 0;
            cam.orthographicSize = onPlayerZoom;
        }
        transform.position += CameraMove;
        cam.orthographicSize += CameraZoom;

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) onPlayer = !onPlayer;
    }
}
