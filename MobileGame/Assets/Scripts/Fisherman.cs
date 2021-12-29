using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fisherman : Unit
{
    public override IEnumerator OnEndTurn()
    {
        actioning = true;
        bool noUnits = true;

        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                else if (CheckFoUnit(x, y))
                {
                    noUnits = false;
                }
            }
        }

        if (noUnits)
        {
            List<GameObject> allies = GetAllies();
            allies.Remove(gameObject);
            for(int i = 0; i < 2; i++)
            {
                float buffTimer = 0;
                if (allies.Count > 0)
                {
                    StartCoroutine(Jiggle());
                    int randomUnitIndex = Random.Range(0, allies.Count - 1);
                    GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
                    while (buffTimer <= buffTime)
                    {
                        newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                            Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                        buffTimer += Time.deltaTime;
                        yield return null;
                    }
                    Destroy(newBuff);
                    allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff * level;
                    allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff * level;
                    StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().BuffJuice(1));
                    StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().Jiggle());
                    allies.Remove(allies[randomUnitIndex]);
                }
            }
        }
        yield return StartCoroutine(base.OnEndTurn());
    }

    private bool CheckFoUnit(int x, int y)
    {
        bool check = false;
        GameSquare square = null;
        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), playerTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), enemyTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        if (square != null && square.occupied) check = true;
        return check;
    }
}
