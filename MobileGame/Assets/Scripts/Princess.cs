using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Princess : Unit
{
    bool stableboy = false;
    [SerializeField]
    GameObject heart;

    public override IEnumerator OnStartOfBattle()
    {
        stableboy = false;
        actioning = true;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                else
                {
                    CheckForStableboy(x, y);
                }
            }
        }

        if (stableboy)
        {
            StartCoroutine(Jiggle());
            List<GameObject> enemies = GetEnemies();
            foreach(GameObject u in enemies)
            {
                StartCoroutine(Buff(u));
                yield return new WaitForSeconds(0.2f);
            }
            float timer = 0;
            while (timer < buffTime)
            {
                timer += Time.deltaTime;
                yield return null;
            }
        }

        yield return StartCoroutine(base.OnStartOfBattle());
    }

    private IEnumerator Buff(GameObject u)
    {
        GameObject newBuff = Instantiate(heart, transform.position, Quaternion.identity);
        float buffTimer = 0;
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, u.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
            Mathf.Lerp(transform.position.y, u.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }
        u.GetComponent<Unit>().health -= Mathf.RoundToInt(u.GetComponent<Unit>().health * (0.2f * level - 0.1f));

        Destroy(newBuff);
        u.GetComponent<Unit>().CollisionJiggle();
    }

    private void CheckForStableboy(int x, int y)
    {
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
        if (square != null && square.occupied && square.occupier.name.Contains("Stableboy")) stableboy = true;
    }

}
