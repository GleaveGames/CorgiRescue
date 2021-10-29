using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTrap : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    public bool triggered;
    private bool fired;
    [SerializeField]
    private float shotPower;
    private float[] walls;
    private int dir;
    [SerializeField]
    private Sprite firedSprite;
    Coroutine coroutine;
    int RotationDir;
    [SerializeField]
    LayerMask tiles;
    [SerializeField]
    float triggerDelay = 0.35f;

    void Start()
    {
        RotationDir = Random.Range(1, 5);
        if (RotationDir == 1) transform.up = Vector2.down;
        else if (RotationDir == 2) transform.up = Vector2.right;
        else if (RotationDir == 3) transform.up = Vector2.down;
        else if (RotationDir == 4) transform.up = Vector2.left;
        RaycastHit2D wall = ClosestWall(transform.up);
        transform.position = wall.point;
        transform.up = -transform.up;
    }


    private void Update()
    {
        if (!triggered)
        {
            RaycastHit2D hit = ClosestRaycast(transform.up);
            if (hit.collider.CompareTag("PickupItems") || hit.collider.CompareTag("Player")) 
            {
                triggered = true;
            }
            else Debug.Log(hit.transform.gameObject.tag);
        }

        else
        {
            if (!fired)
            {
                StartCoroutine(Fire());
                fired = true;
            }
        }
    }

    protected RaycastHit2D ClosestRaycast(Vector2 direction)
    {
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction);
        RaycastHit2D closestValidHit = new RaycastHit2D();
        foreach (RaycastHit2D hit in hits)
        {
            if (hit.transform.gameObject != gameObject && (closestValidHit.collider == null || closestValidHit.distance > hit.distance))
            {
                closestValidHit = hit;
            }
        }
        return closestValidHit;
    }

    private IEnumerator Fire() 
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(triggerDelay);
        GetComponent<SpriteRenderer>().sprite = firedSprite;
        Vector3 spawnpoint = transform.position;
        spawnpoint += transform.up;
        GameObject b = Instantiate(bullet, spawnpoint, transform.rotation);
        b.GetComponent<Rigidbody2D>().AddForce(transform.up * shotPower, ForceMode2D.Impulse);
    }

    private RaycastHit2D ClosestWall(Vector2 direction)
    {
        //working YOU MUST USE A DISTANCE FOR LAYERMASK TO WORK
        RaycastHit2D closestWall = Physics2D.Raycast(transform.position, direction, 9999, tiles);
        return closestWall;
    }
}
