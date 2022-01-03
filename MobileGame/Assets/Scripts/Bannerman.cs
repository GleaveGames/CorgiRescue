using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bannerman : Unit
{
    bool didBuff = false;
    public override IEnumerator OnEndTurn()
    {
        actioning = true;
        if(transform.position.y == 0)
        {
            List<GameObject> allies = GetAllies();
            allies.Remove(gameObject);
            float buffTimer = 0;
            for (int i =0;i<allies.Count;i++)
            {
                GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
                if (allies[i].GetComponent<Unit>().quality != 2) continue;
                StartCoroutine(Jiggle());

                while (buffTimer <= buffTime)
                {
                    newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[i].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                        Mathf.Lerp(transform.position.y, allies[i].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                    buffTimer += Time.deltaTime;
                    yield return null;
                }
                didBuff = true;
                Destroy(newBuff);
                allies[i].GetComponent<Unit>().attack += attackBuff * level;
                allies[i].GetComponent<Unit>().health += healthBuff * level;
                StartCoroutine(allies[i].GetComponent<Unit>().BuffJuice(3));
                StartCoroutine(allies[i].GetComponent<Unit>().Jiggle());
            }
            
        }
        didBuff = false;
        yield return StartCoroutine(base.OnEndTurn());
    }
}
