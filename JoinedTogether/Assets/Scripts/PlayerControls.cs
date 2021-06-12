using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : Penguin
{
    protected override void Start()
    {
        base.Start();
    }

    public override void Movement()
    {
        base.Movement();
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move.x -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move.x += moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move.y -= moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move.y += moveSpeed * Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            if (hasEgg) 
            {
                DropEgg();
            }
            else 
            {
                PickUpEgg();
            }
        }
        
    }

}
