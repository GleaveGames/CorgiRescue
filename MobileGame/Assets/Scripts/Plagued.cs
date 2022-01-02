using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plagued : Unit
{
    public override IEnumerator OnSell()
    {
        actioning = true;
        List<GameObject> allies = GetAllies();
        allies.Remove(gameObject);
        float buffTimer = 0;
        if (allies.Count > 0)
        {
            int randomUnitIndex = Random.Range(0, allies.Count - 1);
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(allies[randomUnitIndex].transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(allies[randomUnitIndex].transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff * level;
            allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff * level;
            StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().BuffJuice(3));
            StartCoroutine(allies[randomUnitIndex].GetComponent<Unit>().Jiggle());
        }

        yield return StartCoroutine(base.OnSell());
    }
}
