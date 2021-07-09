using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penguin : Living
{
    [SerializeField]
    private bool triggered;
    private Vector3 rot;
    // Start is called before the first frame update
    private float timer;
    [SerializeField]
    private float rotateTime;
    private RaycastHit2D hit;
    private Vector3 quaternion;
    private float angle;
    private float offset;
    private Vector3 tempDir;
    [SerializeField]
    private float lookDist = 6;
    private bool GotPlayerRot;
    private Vector3 initRot;
    public float rotationSpeed;
    private float startTime;

    /*
     * Concept
     * Look around CHECK
     * If player is close enough turn towards player and nothing is in the way CHECK
     * If spotted charge towards player until hitting wall
     * then return to idle and repeat
     * */


    // Update is called once per frame
    protected override void Update()
    {
        if (!triggered)
        {
            hit = Physics2D.Raycast(transform.position, transform.up);
            RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, player.transform.position - transform.position);

            //Debug.Log(hit.collider.name);
            if (hit.collider.tag == "Player")
            {
                triggered = true;
                tempDir = player.transform.position - transform.position;
            }
            if(Vector2.Distance(transform.position, player.transform.position) < lookDist && hitPlayer.collider.tag == "Player")
            {
                if (!GotPlayerRot)
                {
                    initRot = transform.eulerAngles;
                    GotPlayerRot = true;
                }
                Vector2 dir = player.transform.position - transform.position;
                float playerAngle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                quaternion = new Vector3(0, 0, playerAngle + angle - 90);
                float fracComplete = (Time.time - startTime) / rotationSpeed;
                rot = Vector3.Lerp(transform.eulerAngles, quaternion, fracComplete*Time.deltaTime);
                transform.eulerAngles = rot;
            }
            else if (timer > rotateTime)
            {
                GotPlayerRot = false;
                timer = 0;
                quaternion = new Vector3(0, 0, Random.Range(1, 5) * 90 + angle + offset);
            }
            else
            {
                GotPlayerRot = false;
                rot = Vector3.Lerp(transform.eulerAngles, quaternion, 0.02f);
                //child.transform.eulerAngles = newRot;
                transform.eulerAngles = rot;
                timer += Time.deltaTime;
            }
        }
        //
        else
        {
            hit = Physics2D.Raycast(transform.position, transform.up);
            if (Vector2.Distance(hit.point, transform.position) > 0.4 && hit.collider.gameObject != gameObject)
            {
                AttackSound();
                transform.position +=  tempDir.normalized * speed * Time.deltaTime;
                ChangeAnimationState("PenguinDive");
            }
            else
            {
                //transform.rotation = Quaternion.identity;
                ChangeAnimationState("PenguinIdle");
                triggered = false;
            }
        }
    }
}
