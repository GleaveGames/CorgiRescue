using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lance : Unit
{
    public override IEnumerator OnAttack()
    {
        actioning = true;

        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        attack += attackBuff * level;
        health += healthBuff * level;
        StartCoroutine(BuffJuice(2, attackBuff*level, 0));
        StartCoroutine(Jiggle());
        actioning = false;

        yield return StartCoroutine(base.OnAttack());
    }
}
