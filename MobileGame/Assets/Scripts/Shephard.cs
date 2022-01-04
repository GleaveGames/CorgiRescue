using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shephard : Unit
{
    public IEnumerator ShephardBuff(GameObject buffedUnit)
    {
        actioning = true;
        float buffTimer = 0;
        StartCoroutine(Jiggle());
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, buffedUnit.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, buffedUnit.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        buffedUnit.GetComponent<Unit>().attack += attackBuff * level;
        buffedUnit.GetComponent<Unit>().health += healthBuff * level;
        StartCoroutine(buffedUnit.GetComponent<Unit>().Jiggle());
        StartCoroutine(buffedUnit.GetComponent<Unit>().BuffJuice(3));
        actioning = false;
        yield return null;
    }

}
