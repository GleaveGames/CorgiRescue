using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bannerman : Unit
{
    bool didBuff = false;
    public override IEnumerator OnStartOfTurn()
    {
        actioning = true;
        Collider2D col = Physics2D.OverlapPoint(transform.position, playerTiles);
        if(col.transform.position.y == 0)
        {
            List<GameObject> allies = GetAlliesBarracks();
            allies.Remove(gameObject);
            for (int i =0;i<level;i++)
            {
                if (allies.Count <= 0)
                {
                    break;
                }
                GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
                StartCoroutine(Jiggle());
                float buffTimer = 0;
                int r = Random.Range(0, allies.Count);

                while (buffTimer <= buffTime)
                {
                    newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[r].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                        Mathf.Lerp(transform.position.y, allies[r].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                    buffTimer += Time.deltaTime;
                    yield return null;
                }
                didBuff = true;
                Destroy(newBuff);
                allies[r].GetComponent<Unit>().attack += attackBuff;
                allies[r].GetComponent<Unit>().health += healthBuff;
                StartCoroutine(allies[r].GetComponent<Unit>().BuffJuice(3));
                StartCoroutine(allies[r].GetComponent<Unit>().Jiggle());
                allies.Remove(allies[r]);
            }
        }
        if (didBuff) StartCoroutine(Jiggle());
        didBuff = false;
        yield return StartCoroutine(base.OnStartOfTurn());
    }

    public List<GameObject> GetAlliesBarracks()
    {
        List<GameObject> allies = new List<GameObject>();

        if (playerUnit)
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (unit.playerUnit && unit.health > 0 && unit.quality == 2) allies.Add(gc.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (!unit.playerUnit && unit.health > 0) allies.Add(gc.transform.GetChild(i).gameObject);
            }
        }

        return allies;
    }

}
