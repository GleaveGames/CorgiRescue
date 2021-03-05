using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBat : MonoBehaviour
{
    private GameObject player;
    public List<GameObject> bats;
    private Vector3 targetPos;
    private float timer;
    [SerializeField]
    private float offset = 90;
    [SerializeField]
    private GameObject bat;
    [SerializeField]
    private LayerMask ignoreMe;
    [SerializeField]
    private List<GameObject> coreBats;
    [SerializeField]
    private float moveSpeed;
    [SerializeField]
    private bool[] walls;
    private Vector2 tempdir;
    [SerializeField]
    private float wallRange;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, player.transform.position - transform.position);
        //loop through hits in order, if when it hits the player the previous hit was a bat then it can see the player;
        if (hit.collider.name.Contains("Player"))
        {
            transform.position = Vector2.MoveTowards(transform.position, transform.position + transform.up, moveSpeed * Time.deltaTime);
            targetPos = player.transform.position;
            targetPos.x -= transform.position.x;
            targetPos.y -= transform.position.y;
            float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + offset));

            int j = Random.Range(0, bats.Count);
            if (bats[j] != null)
            {
                if (timer < 0)
                {
                    //bats[j].GetComponent<Bat>().inSwarm = false;
                    //bats[j].GetComponent<Bat>().triggered = true;
                    //bats[j].transform.parent = null;
                    timer = 2;
                    GameObject newBat = Instantiate(bat, bats[j].transform.position, transform.rotation);
                    newBat.GetComponent<Bat>().inSwarm = false;
                    newBat.GetComponent<Bat>().triggered = true;
                }
            }
            timer -= Time.deltaTime;
        }
        else
        {
            RandomlyMove();
        }

        for(int core = 0; core < coreBats.Count; core++)
        {
            if(coreBats[core] == null)
            {
                coreBats.RemoveAt(core);
            }
        }
        if(coreBats.Count < 2)
        {
            Debug.Log("unleash");
            UnleashAllBats();
        }


        
    }

    private void UnleashAllBats()
    {
        for (int i = 0; i < bats.Count; i++)
        {
            if(bats[i] != null)
            {
                bats[i].GetComponent<Bat>().inSwarm = false;
                bats[i].GetComponent<Bat>().triggered = true;
                bats[i].transform.parent = null;
            }
        }
        Destroy(gameObject, 1);
    }

    private void RandomlyMove()
    {
        RaycastHit2D[] hitLeft = Physics2D.RaycastAll(transform.position, Vector2.left, wallRange);
        Debug.DrawRay(transform.position, Vector2.left);
        walls[0] = false;
        if (hitLeft.Length > 0)
        {
            for (int f = 0; f < hitLeft.Length; f++)
            {
                if (hitLeft[f].collider.gameObject.CompareTag("Wall") || hitLeft[f].collider.gameObject.CompareTag("Rock"))
                {
                    walls[0] = true;
                }
            }
        }
        RaycastHit2D[] hitUp = Physics2D.RaycastAll(transform.position, Vector2.up, wallRange);
        Debug.DrawRay(transform.position, Vector2.up);
        walls[1] = false;
        if (hitUp.Length > 0)
        {
            for (int p = 0; p < hitUp.Length; p++)
            {
                if (hitUp[p].collider.gameObject.CompareTag("Wall") || hitUp[p].collider.gameObject.CompareTag("Rock"))
                {
                    walls[1] = true;
                }
            }
        }
        RaycastHit2D[] hitRight = Physics2D.RaycastAll(transform.position, Vector2.right, wallRange);
        Debug.DrawRay(transform.position, Vector2.right);
        walls[2] = false;
        if (hitRight.Length > 0)
        {

            for (int b = 0; b < hitRight.Length; b++)
            {
                if (hitRight[b].collider.gameObject.CompareTag("Wall") || hitRight[b].collider.gameObject.CompareTag("Rock"))
                {
                    walls[2] = true;
                }
            }
        }
        RaycastHit2D[] hitDown = Physics2D.RaycastAll(transform.position, Vector2.down, wallRange);
        Debug.DrawRay(transform.position, Vector2.down);
        walls[3] = false;
        if (hitDown.Length > 0)
        {
            for (int m = 0; m < hitDown.Length; m++)
            {
                if (hitDown[m].collider.gameObject.CompareTag("Wall") || hitDown[m].collider.gameObject.CompareTag("Rock"))
                {
                    walls[3] = true;
                }
            }
        }
        //Vector2.MoveTowards(transform.position, tempdir, runsp);
        if (walls[0] && walls[1])
        {
            tempdir = new Vector2(Random.Range(0f, 1f), Random.Range(-1f, 0f));
            timer = 0;
        }
        else if (walls[1] && walls[2])
        {
            tempdir = new Vector2(Random.Range(-1f, 0f), Random.Range(-1f, 0f));
            timer = 0;
        }
        else if (walls[2] && walls[3])
        {
            tempdir = new Vector2(Random.Range(-1f, 0f), Random.Range(0f, 1f));
            timer = 0;
        }
        else if (walls[3] && walls[0])
        {
            tempdir = new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f));
            timer = 0;
        }
        else if (walls[0])
        {
            tempdir = new Vector2(1, Random.Range(-1f, 1f));
            timer = 0;
        }
        else if (walls[1])
        {
            tempdir = new Vector2(Random.Range(-1f, 1f), -1);
            timer = 0;
        }
        else if (walls[2])
        {
            tempdir = new Vector2(-1, Random.Range(-1f, 1f));
            timer = 0;
        }
        else if (walls[3])
        {
            tempdir = new Vector2(Random.Range(-1f, 1f), 1);
            timer = 0;
        }
        else if (tempdir.magnitude < 0.1) tempdir = new Vector2(Random.Range(-2f, 2f), Random.Range(-2f, 2f));
        transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, transform.position.y) + tempdir, moveSpeed * Time.deltaTime);
        Quaternion rot = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward, tempdir), 0.02f);
        transform.rotation = rot;
    }
}
