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

    private void Update()
    {
        if (!triggered)
        {
            //dirs ::: 0 = left; 1 = up; 2 = right; 3 = down;
            Vector2 pos = transform.position;
            RaycastHit2D los;
            if (dir == 0)
            {
                los = Physics2D.Raycast(pos + Vector2.left, Vector2.left);
            }
            else if(dir == 1)
            {
                los = Physics2D.Raycast(pos + Vector2.up, Vector2.up);
            }
            else if (dir == 2)
            {
                los = Physics2D.Raycast(pos + Vector2.right, Vector2.right);
            }
            else
            {
                los = Physics2D.Raycast(pos + Vector2.down, Vector2.down);
            }
            if (!los.collider.name.Contains("Walls") && !los.collider.name.Contains("Rock") && !los.collider.name.Contains("Obisidian") &&
                !los.collider.name.Contains("Tilemap") && !los.collider.name.Contains("Trap") && !los.collider.name.Contains("EndHole") &&
                !los.collider.name.Contains("Plate") && !los.collider.name.Contains("Spike") && !los.collider.name.Contains("pickaxe"))
            {
                triggered = true;
            }
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

    private void Start()
    {
        Vector2 pos = transform.position;  
        walls = new float[4];
        //Doing this pos thing so it doesn't raycast itself cus I sitll haven't done layers altohugh didn't work anyway
        //currently doesn't register if it's up against a wall
        RaycastHit2D hitLeft = Physics2D.Raycast(pos, Vector2.left);
        walls[0] = 0;
        if (hitLeft.collider.gameObject.CompareTag("Wall") || hitLeft.collider.gameObject.CompareTag("Rock") || hitLeft.collider.gameObject.CompareTag("Obsidian"))
        {
            walls[0] = hitLeft.distance;
        }
        RaycastHit2D hitUp = Physics2D.Raycast(pos , Vector2.up);
        walls[1] = 0;       
        if (hitUp.collider.gameObject.CompareTag("Wall") || hitUp.collider.gameObject.CompareTag("Rock") || hitUp.collider.gameObject.CompareTag("Obsidian"))
        {
            walls[1] = hitUp.distance;
        }
        RaycastHit2D hitRight = Physics2D.Raycast(pos, Vector2.right);
        walls[2] = 0;
        if (hitRight.collider.gameObject.CompareTag("Wall") || hitRight.collider.gameObject.CompareTag("Rock") || hitRight.collider.gameObject.CompareTag("Obsidian"))
        {
            walls[2] = hitRight.distance;
        }
        RaycastHit2D hitDown = Physics2D.Raycast(pos, Vector2.down);
        walls[3] = 0;
        if (hitDown.collider.gameObject.CompareTag("Wall") || hitDown.collider.gameObject.CompareTag("Rock") || hitDown.collider.gameObject.CompareTag("Obsidian"))
        {
            walls[3] = hitDown.distance;
        }
        float biggest = walls[0];
        dir = 0;
        for (int i = 1; i < walls.Length; i++)
        {
            if(walls[i] > biggest)
            {
                biggest = walls[i];
                dir = i;
            }
        }

        //dirs ::: 0 = left; 1 = up; 2 = right; 3 = down;
        if(dir == 0)
        {
            transform.Rotate(0f, 0f, 90f);
        }
        else if (dir == 2)
        {
            transform.Rotate(0f, 0f, -90f);
        }
        else if(dir == 3)
        {
            transform.Rotate(0f, 0f, 180f);
        }
    }

    private IEnumerator Fire() 
    {
        GetComponent<AudioSource>().Play();
        yield return new WaitForSeconds(0.35f);
        GetComponent<SpriteRenderer>().sprite = firedSprite;
        Vector3 spawnpoint = transform.position;
        spawnpoint += transform.up;
        GameObject b = Instantiate(bullet, spawnpoint, transform.rotation);
        b.GetComponent<Rigidbody2D>().AddForce(transform.up * shotPower, ForceMode2D.Impulse);
    }
}
