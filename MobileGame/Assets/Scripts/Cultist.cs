using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cultist : Unit
{
    [SerializeField]
    Sprite blessing;
    public override IEnumerator OnEndTurn()
    {
        bool anyChurch = false;
        List<GameObject> allies = GetAllies();
        allies.Remove(gameObject);
        foreach (GameObject u in allies) if (u.GetComponent<Unit>().quality == 3) anyChurch = true;
        if (!anyChurch)
        {
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            newBuff.GetComponent<SpriteRenderer>().sprite = blessing;
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
            StartCoroutine(BuffJuice(3));
            StartCoroutine(Jiggle());
        }

        yield return StartCoroutine(base.OnEndTurn());
    }
}
