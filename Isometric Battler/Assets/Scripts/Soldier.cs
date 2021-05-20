using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Soldier : NetworkBehaviour
{
    GameManager gm;
    [SerializeField]
    [Range(0.0001f,3)]
    float speed;
    [SerializeField]
    [Range(0.0001f, 3)]
    float attackSpeed;
    int[,] tiles;
    Vector2Int currentTile;
    Coroutine coroutine;
    int[] dir;
    Vector2 movepos;
    bool moving, attacking;
    GameObject closestEnemy;
    [SerializeField]
    float attackRange;
    [SerializeField]
    int damage;

    private void Start()
    {
        if (!isServer)
        {
            this.enabled = false;
        }
        gm = FindObjectOfType<GameManager>();
        tiles = gm.tiles;
        dir = new int[8];
        movepos = transform.position;
    }

    private void GetCurrentTile()
    {
        float yRuff = 0.5f * (transform.position.y / 0.815f - transform.position.x / 1.415f + gm.boundsY - 1);
        float xRuff = transform.position.x / 1.4f + 0.5f * (transform.position.y / 0.815f - transform.position.x / 1.415f + gm.boundsY - 1);
        int y = Mathf.RoundToInt(yRuff);
        int x = Mathf.RoundToInt(xRuff);
        currentTile = new Vector2Int(x, y);
    }

    private void GetAvailableTiles()
    {
        if (tiles[currentTile.x + 1, currentTile.y + 1] == 1 || tiles[currentTile.x + 1, currentTile.y + 1] == 2) dir[0] = 1;
        else dir[0] = 0;
        if (tiles[currentTile.x + 1, currentTile.y] == 1 || tiles[currentTile.x + 1, currentTile.y] == 2) dir[1] = 1;
        else dir[1] = 0;
        if (tiles[currentTile.x + 1, currentTile.y - 1] == 1 || tiles[currentTile.x + 1, currentTile.y - 1] == 2) dir[2] = 1;
        else dir[2] = 0;
        if (tiles[currentTile.x, currentTile.y - 1] == 1 || tiles[currentTile.x, currentTile.y - 1] == 2) dir[3] = 1;
        else dir[3] = 0;
        if (tiles[currentTile.x - 1, currentTile.y - 1] == 1 || tiles[currentTile.x - 1, currentTile.y - 1] == 2) dir[4] = 1;
        else dir[4] = 0;
        if (tiles[currentTile.x - 1, currentTile.y] == 1 || tiles[currentTile.x - 1, currentTile.y] == 2) dir[5] = 1;
        else dir[5] = 0;
        if (tiles[currentTile.x - 1, currentTile.y + 1] == 1 || tiles[currentTile.x - 1, currentTile.y + 1] == 2) dir[6] = 1;
        else dir[6] = 0;
        if (tiles[currentTile.x, currentTile.y + 1] == 1 || tiles[currentTile.x, currentTile.y + 1] == 2) dir[7] = 1;
        else dir[7] = 0;
    }

    private void GetMovePos(int dir) 
    {
        movepos = transform.position;
        if (dir == 0)
        {
            movepos.y += 1.63f;
        }
        else if (dir == 1)
        {
            movepos.y += 0.815f;
            movepos.x += 1.415f;
        }
        else if (dir == 2)
        {
            movepos.x += 2.83f;
        }
        else if (dir == 3)
        {
            movepos.x += 1.415f;
            movepos.y -= 0.815f;
        }
        else if (dir == 4)
        {
            movepos.y -= 1.63f;
        }
        else if (dir == 5)
        {
            movepos.y -= 0.815f;
            movepos.x -= 1.415f;
        }
        else if (dir == 6)
        {
            movepos.x -= 2.83f;
        }
        else if(dir == 7) 
        {
            movepos.x -= 1.415f;
            movepos.y += 0.815f;
        }
        else 
        {
            Debug.Log("No move pos");
            movepos = transform.position;
        }
    }
    
    private Vector2 GetTilePos(int dir) 
    {
        Vector2 pos = transform.position;
        if (dir == 0)
        {
            pos.y += 1.63f;
        }
        else if (dir == 1)
        {
            pos.y += 0.815f;
            pos.x += 1.415f;
        }
        else if (dir == 2)
        {
            pos.x += 2.83f;
        }
        else if (dir == 3)
        {
            pos.x += 1.415f;
            pos.y -= 0.815f;
        }
        else if (dir == 4)
        {
            pos.y -= 1.63f;
        }
        else if (dir == 5)
        {
            pos.y -= 0.815f;
            pos.x -= 1.415f;
        }
        else if (dir == 6)
        {
            pos.x -= 2.83f;
        }
        else if(dir == 7) 
        {
            pos.x -= 1.415f;
            pos.y += 0.815f;
        }
        else 
        {
            Debug.Log("No move pos");
            pos = transform.position;
        }
        return pos;
    }

    private IEnumerator Attack() 
    {
        movepos = closestEnemy.transform.position;
        Vector2 originalPos = transform.position;
        while (new Vector2(transform.position.x, transform.position.y) != movepos && closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, movepos, attackSpeed * Time.deltaTime);
            yield return null;
        }
        if(closestEnemy != null) { 
            closestEnemy.GetComponent<CharacterStats>().health -= damage;
        }
        CheckForEnemies();
        while (new Vector2(transform.position.x, transform.position.y) != originalPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, attackSpeed/2 * Time.deltaTime);
            yield return null;
        }

        moving = false;
        attacking = false;
    }

    private IEnumerator Move() 
    {
        moving = true;
        attacking = false;
        yield return new WaitForSeconds(0.4f);
        //starting up going clockwise;
        GetCurrentTile();
        GetAvailableTiles();
        //RandomMove();
        MoveTowardsEnemy();
        while(new Vector2(transform.position.x, transform.position.y) != movepos) 
        {
            transform.position = Vector2.MoveTowards(transform.position, movepos, speed * Time.deltaTime);
            yield return null;
        }
        CheckForEnemies();
        moving = false;
        attacking = false;
    }

    private void MoveTowardsEnemy() 
    {
        //This can definitely be cleaned up theree's a bunch of recursive code
        if (closestEnemy == null)
        {
            RandomMove();
        }
        else
        {
            Vector2 targetpos = transform.position + 2.1f * (closestEnemy.transform.position - transform.position).normalized;
            int closestdir = 0;
            float closestdist = 999;
            for (int i = 0; i < dir.Length; i++)
            {
                if (dir[i] == 1)
                {
                    if (closestdist > Vector2.Distance(targetpos, GetTilePos(i)))
                    {
                        closestdist = Vector2.Distance(targetpos, GetTilePos(i));
                        closestdir = i;
                    }
                }
            }
            GetMovePos(closestdir);
        }
    }


    private void RandomMove() 
    {
        int sum = 0;
        for (int i = 0; i < dir.Length; i++)
        {
            if (dir[i] == 1) sum++;
        }
        int choice = Random.Range(0, sum);
        sum = 0;
        for (int i = 0; i < dir.Length; i++)
        {
            if (dir[i] == 1)
            {
                if (sum == choice)
                {
                    GetMovePos(i);
                    break;
                }
                else sum++;
            }
        }
    }

    private void Update()
    {
        if (!moving && !attacking)
        {
            if (closestEnemy != null)
            {
                if (Vector2.Distance(transform.position, closestEnemy.transform.position) < attackRange)
                {
                    StartCoroutine(Attack());
                    attacking = true;
                }
                else
                {
                    StartCoroutine(Move());
                }
            }
            else
            {
                StartCoroutine(Move());
            }
        }
    }

    private void CheckForEnemies()
    {
        closestEnemy = null;
        float closestDistance = 99;
        foreach (Team team in gm.teams) 
        {
            if(team.color != GetComponent<SpriteRenderer>().color) 
            {
                if(closestEnemy == null)
                {
                    //code below breaks when there aren't any enemies
                    if(team.things.Count > 0) 
                    {
                        closestEnemy = team.things[0];
                        closestDistance = Vector2.Distance(transform.position, closestEnemy.transform.position);
                    }
                    else 
                    {
                        Debug.Log("No enemies");
                    }
                }
                foreach(GameObject thing in team.things) 
                {
                    if (thing != null) 
                    {
                        if (Vector2.Distance(transform.position, thing.gameObject.transform.position) < closestDistance)
                        {
                            closestEnemy = thing;
                            closestDistance = Vector2.Distance(transform.position, thing.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }
}
