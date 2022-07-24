using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 move;
    public float speed;

    

    void Update()
    {
        move = new Vector2(0, 0);
        if (Input.GetKey(KeyCode.LeftArrow)) move.x --; 
        if (Input.GetKey(KeyCode.RightArrow)) move.x ++; 
        if (Input.GetKey(KeyCode.DownArrow)) move.y --; 
        if (Input.GetKey(KeyCode.UpArrow)) move.y ++;

        
    }

    private void FixedUpdate()
    {
        if(move.magnitude != 0)
        {
            Vector2 moveFinal = new Vector2(move.x, move.y);
            moveFinal = moveFinal.normalized;
            moveFinal *= speed;
            transform.position += new Vector3(moveFinal.x * Time.deltaTime, moveFinal.y*Time.deltaTime, 0);
        }
    }
}
