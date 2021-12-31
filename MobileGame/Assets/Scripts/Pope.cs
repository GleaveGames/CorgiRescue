using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pope : Unit
{
    bool jiggled = false;
    [SerializeField]
    Sprite blessing;

    public override IEnumerator OnBuy()
    {
        actioning = true;
        List<GameObject> buffs = new List<GameObject>();
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
                    StartCoroutine(CheckForUnitAndBuff(x, y));
                }
            }
        }

        jiggled = false;
        yield return StartCoroutine(base.OnBuy());
    }

    private IEnumerator CheckForUnitAndBuff(int x, int y)
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
        //Give Buff
        if (square != null && square.occupied)
        {
            if (!jiggled) StartCoroutine(Jiggle());
            jiggled = true;
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            newBuff.GetComponent<SpriteRenderer>().sprite = blessing;
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, square.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, square.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(newBuff);
            square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().health += healthBuff * level;
            StartCoroutine(square.occupier.GetComponent<Unit>().BuffJuice(1));
            StartCoroutine(square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().Jiggle());
        }
        else
        {
            yield return null;
        }
    }
}
