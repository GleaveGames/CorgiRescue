using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : Unit
{
    public override IEnumerator OnBuy()
    {
        actioning = true;
        List<GameObject> allies = GetAllies();
        allies.Remove(gameObject);
        if (allies.Count > 0)
        {
            float buffTimer = 0;
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
            StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().BuffJuice(3,attackBuff*level, healthBuff*level));
            StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().Jiggle());
        }
        yield return StartCoroutine(base.OnBuy());
    }
}
