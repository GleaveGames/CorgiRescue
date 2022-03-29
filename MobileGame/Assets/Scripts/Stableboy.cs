using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stableboy : Unit
{
    bool princess;
    [SerializeField]
    GameObject heart;
    public override IEnumerator OnDie()
    {
        princess = false;
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
                    CheckForPrincess(x, y);
                }
            }
        }
        if (princess)
        {
            StartCoroutine(Jiggle());
            List<GameObject> enemies = GetEnemies();
            foreach (GameObject u in enemies)
            {
                if (u.GetComponent<Unit>().health > 0)
                {
                    StartCoroutine(GiveBuff(u));
                    yield return new WaitForSeconds(0.2f);
                }
            }
                
            yield return new WaitForSeconds(buffTime * 2);
        }
        actioning = false;
        yield return StartCoroutine(base.OnDie());
    }


    private IEnumerator GiveBuff(GameObject u)
    {
        GameObject newBuff = Instantiate(heart, transform.position, Quaternion.identity);
        newBuff.GetComponent<Buff>().good = false;
        float buffTimer = 0;
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, u.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
            Mathf.Lerp(transform.position.y, u.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }
        u.GetComponent<Unit>().health += healthBuff * level;
        if (u.GetComponent<Unit>().health <= 0) StartCoroutine(u.GetComponent<Unit>().OnDie());
        else StartCoroutine(u.GetComponent<Unit>().OnHurt());
        Destroy(newBuff);
        u.GetComponent<Unit>().CollisionJiggle();
        ShowDamage(-healthBuff * level, u.transform.position);
    }

    private void CheckForPrincess(int x, int y)
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
        if (square != null && square.occupied && square.occupier.name.Contains("Princess")) princess = true;
    }
}
