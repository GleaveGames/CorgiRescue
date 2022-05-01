using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        if((playerUnit && transform.position.y == -2.5) || (!playerUnit && transform.position.y == 3.75)){
            StartCoroutine(Jiggle());
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
            StartCoroutine(Jiggle());
            StartCoroutine(BuffJuice(2, attackBuff*level, 0));
        }
        yield return StartCoroutine(base.OnStartOfBattle());
    }
}
