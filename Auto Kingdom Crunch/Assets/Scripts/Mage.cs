using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : Unit
{
    [SerializeField]
    Sprite fireball;
    public override IEnumerator OnAttack()
    {
        actioning = true;
          
        //throw a fireball at a random enemy
        List<GameObject> enemies = GetEnemies();
        
        StartCoroutine(Jiggle());
        int randomUnitIndex = Random.Range(0, enemies.Count - 1);
        GameObject newBuff = Instantiate(RangedAttack, transform.position, Quaternion.identity);
        newBuff.GetComponent<SpriteRenderer>().sprite = fireball;
        newBuff.GetComponent<Buff>().good = false;
        float buffTimer = 0;
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, enemies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, enemies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        enemies[randomUnitIndex].GetComponent<Unit>().health += healthBuff * level;
        if (enemies[randomUnitIndex].GetComponent<Unit>().health <= 0) StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnDie());
        else StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnHurt());
        StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().CollisionJiggle());
        ShowDamage(-healthBuff * level, enemies[randomUnitIndex].transform.position);
        while (enemies[randomUnitIndex] != null && enemies[randomUnitIndex].GetComponent<Unit>().actioning) yield return null;
        yield return StartCoroutine(base.OnAttack());
    }
}
