using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : Penguin
{
    bool gotEgg;
    public GameObject egg;
    [SerializeField]
    float eggSpeed;
    [SerializeField]
    float pickUpRange;
    [SerializeField]
    Transform eggPos;
    float normalSpeed;

    private void Start()
    {
        normalSpeed = (float)moveSpeed;
        rb = GetComponent<Rigidbody2D>();
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
            if(gotEgg)
            {
                //dropEgg;
                egg.transform.parent = null;
                gotEgg = false;
                egg.GetComponent<SpriteRenderer>().sortingOrder++;
                moveSpeed = normalSpeed;
            }
            else 
            {
                if (Vector2.Distance(transform.position, egg.transform.position) < pickUpRange)
                {
                    //pickUp
                    moveSpeed = eggSpeed;
                    egg.transform.position = eggPos.position;
                    egg.transform.parent = transform;
                    egg.GetComponent<SpriteRenderer>().sortingOrder--;
                    gotEgg = true;
                }
            }
        }
        
    }

}
