using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paladin : Unit
{
    public override IEnumerator OnDie()
    {
        List<GameObject> allies = GetAllies();
        allies.Remove(gameObject);
        for(int i =0;i < allies.Count; i++)
        {
            if(allies[i].GetComponent<Unit>().quality == 3)
            {
                StartCoroutine(Jiggle());
                GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
                float buffTimer = 0;

                while (buffTimer <= buffTime)
                {
                    newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[i].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                        Mathf.Lerp(transform.position.y, allies[i].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                    buffTimer += Time.deltaTime;
                    yield return null;
                }
                Destroy(newBuff);
                allies[i].GetComponent<Unit>().attack += attackBuff * level;
                allies[i].GetComponent<Unit>().health += healthBuff * level;
                StartCoroutine(allies[i].GetComponent<Unit>().BuffJuice(3));
                StartCoroutine(allies[i].GetComponent<Unit>().Jiggle());
                yield return new WaitForSeconds(buffTime/2);
            }
        }

        yield return StartCoroutine(base.OnDie());
    }
}
