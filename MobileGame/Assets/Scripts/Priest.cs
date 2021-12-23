using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Priest : Unit
{
    bool triggered = false;
    GameObject unitAhead;
    public override void Update()
    {
        if(unitAhead == null)
        {
            GameSquare square = null;

            if (playerUnit)
            {
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1.25f), playerTiles);
                if (squareCol != null)
                {
                    square = squareCol.GetComponent<GameSquare>();
                }
                if (square != null && square.occupied)
                {
                    unitAhead = square.occupier;
                }
            }
            else
            {
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1.25f), enemyTiles);
                if (squareCol != null)
                {
                    square = squareCol.GetComponent<GameSquare>();
                }
                if (square != null && square.occupied)
                {
                    unitAhead = square.occupier;
                }
            }
        }
        else if(!triggered)
        {
            if(unitAhead.GetComponent<Unit>().health <= 0)
            {
                StartCoroutine(ReSpawn());
                triggered = true;
            }
        }
        base.Update();
    }

    protected IEnumerator ReSpawn()
    {
        Collider2D squareCol;
        GameObject clone;
        if (playerUnit)
        {
            squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1.25f), playerTiles);
            clone = Instantiate(unitAhead, unitAhead.transform.position, Quaternion.identity);
        }
        else
        {
            squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1.25f), enemyTiles);
            clone = Instantiate(unitAhead, unitAhead.transform.position, Quaternion.identity);
        }
        yield return new WaitForSeconds(0.5f);
        GameSquare square = squareCol.GetComponent<GameSquare>();
        square.occupied = true;
        square.occupier = clone;
        clone.GetComponent<Unit>().health = 1;
        clone.GetComponent<Unit>().attack = 1;
        clone.GetComponent<Unit>().level = level;
        clone.GetComponent<Unit>().dead = false;
        clone.GetComponent<SpriteRenderer>().enabled = true;
        clone.transform.GetChild(0).gameObject.SetActive(true);
        clone.transform.GetChild(0).GetChild(6).gameObject.SetActive(false);
        clone.transform.parent = gc.transform;
        if (!playerUnit) gc.enemyUnits.Add(clone);
        else gc.playerUnits.Add(clone);
        gc.allUnits.Add(clone);

        if (gc.Battling)
        {
            while (gc.Battling)
            {
                yield return null;
            }
            if(!clone.GetComponent<Unit>().dead) Instantiate(deathParticles, clone.transform.position, Quaternion.identity);
            if (!playerUnit) gc.enemyUnits.Remove(clone);
            else gc.playerUnits.Remove(clone);
            gc.allUnits.Remove(clone);
            Destroy(clone);
        }
        else
        {
            triggered = false;
        }
    }

    public override IEnumerator OnEndTurn()
    {
        triggered = false;
        return base.OnEndTurn();
    }

    public override IEnumerator OnStartOfTurn()
    {
        triggered = false;
        return base.OnStartOfTurn();
    }
}
