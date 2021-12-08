using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Peasant : Unit
{
    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        int peasants = 0;
        if (playerUnit)
        {
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (x == 0 && y == 1)
                    {
                        continue;
                    }
                    else if(CheckForPeasant(x,y))
                    {
                        peasants++;
                    }
                }
            }
        }
        if (peasants>0)
        {
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(newBuff);
            attack += attackBuff * level;
        }
        yield return StartCoroutine(base.OnStartOfBattle());
    }

    private bool CheckForPeasant(int x, int y)
    {
        bool check = false;
        GameSquare square = null;
        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x, y), playerTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x, y), enemyTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        if (square != null && square.occupied && square.occupier.name.Contains("Peasant")) check = true;
        return check;
    }
}
