using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    RaycastHit hit;
    public float cameraHeight;
    public float cameraSpeed;
    Quaternion rotation;

    void Update()
    {
        Vector3 newpos = transform.position;
        if (Physics.Raycast(transform.position, transform.GetChild(1).transform.forward, out hit))
        {
            float offsetDistance = hit.distance;
            float diff = cameraHeight - offsetDistance;
            newpos.y += diff * 0.3f;
        }
        if (Input.GetKey(KeyCode.W)) newpos += transform.forward;
        if (Input.GetKey(KeyCode.S)) newpos -= transform.forward;
        if (Input.GetKey(KeyCode.A)) newpos -= transform.right;
        if (Input.GetKey(KeyCode.D)) newpos += transform.right;
        transform.position = Vector3.Lerp(transform.position, newpos, cameraSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, cameraSpeed * Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.rotation = rotation;
            rotation = Quaternion.LookRotation(-transform.right);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.rotation = rotation;
            rotation = Quaternion.LookRotation(transform.right);
        }
    }
}
