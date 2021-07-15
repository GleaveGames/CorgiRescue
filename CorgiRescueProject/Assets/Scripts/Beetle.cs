using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beetle : Living
{
    protected bool idle, turning, midturn;
    int RotationDir;

    [SerializeField]
    protected float bumpRange = 0.5f;
    protected AudioSource crawlsound;

    protected override void Start()
    {
        base.Start();
        crawlsound = GetComponent<AudioSource>();
        idle = true;
        Rerotate();
    }

    private void FixedUpdate()
    {
        if (idle) 
        {
            RaycastHit2D hit = ClosestWall(transform.up);
            RaycastHit2D hit1 = Physics2D.Raycast(transform.position - transform.right/3, transform.up, 999, 13);
            RaycastHit2D hit2 = Physics2D.Raycast(transform.position + transform.right/3, transform.up, 999, 13);
            if (Vector2.Distance(hit.point, transform.position) > bumpRange && Vector2.Distance(hit1.point, transform.position) > bumpRange && Vector2.Distance(hit2.point, transform.position) > bumpRange && !turning)
            {
                //move forward;
                transform.position = Vector2.MoveTowards(transform.position, hit.point, speed * Time.deltaTime);
                midturn = false;
            }
            else
            //pause, rotate 90 left or right;
            {
                turning = true;
                if (!midturn) StartCoroutine(Turn());
            }
        }
    }

    protected virtual IEnumerator Turn()
    {       
        Debug.Log("turn in beetle");
        return null;
    }

    protected void Rerotate() 
    {
        RotationDir = Random.Range(1, 5);
        if (RotationDir == 1) transform.up = Vector2.up;
        else if (RotationDir == 2) transform.up = Vector2.right;
        else if (RotationDir == 3) transform.up = Vector2.down;
        else if (RotationDir == 4) transform.up = Vector2.left;
    }
}
