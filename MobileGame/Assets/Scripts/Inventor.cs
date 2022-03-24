using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventor : Unit
{
    public override IEnumerator OnStartOfTurn()
    {
        actioning = true;
        Collider2D col = Physics2D.OverlapPoint(transform.position + new Vector3(1.25f, 0, 0),playerTiles);
        GameSquare square = null;
        if (col != null) square = col.GetComponent<GameSquare>();
        if (square != null && square.occupied && square.occupied)
        {
            StartCoroutine(Jiggle());
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            GameObject unit = square.occupier;
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, unit.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, unit.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(newBuff);
            unit.GetComponent<Unit>().health += healthBuff * level;
            unit.GetComponent<Unit>().attack += attackBuff * level;
            StartCoroutine(unit.GetComponent<Unit>().BuffJuice(3,attackBuff*level, healthBuff*level));
            StartCoroutine(unit.GetComponent<Unit>().Jiggle());
        }

        yield return StartCoroutine(base.OnStartOfTurn());
    }

    /*
    public override IEnumerator OnStartOfTurn()
    {
        List<GameObject> allies = GetAllies();
        foreach(GameObject ally in allies)
        {
            if(ally.GetComponent<Unit>().quality < 3)
            {
                //buff
                StartCoroutine(GiveBuff(ally));
                yield return new WaitForSeconds(0.3f);
            }
        }
        yield return new WaitForSeconds(buffTime);

        yield return StartCoroutine(base.OnStartOfTurn());
    }

    private IEnumerator GiveBuff(GameObject unit)
    {
        StartCoroutine(Jiggle());
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, unit.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, unit.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(newBuff);
        unit.GetComponent<Unit>().health += healthBuff * level;
        unit.GetComponent<Unit>().attack += attackBuff * level;
        StartCoroutine(unit.GetComponent<Unit>().BuffJuice(3));
        StartCoroutine(unit.GetComponent<Unit>().Jiggle());
    }

    */
}
