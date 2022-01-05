﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jester : Unit
{
    [SerializeField]
    Sprite ball;

    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        List<GameObject> enemies = GetEnemies();
        float buffTimer = 0;
        if (enemies.Count > 0)
        {
            StartCoroutine(Jiggle());
            //Throw ball;
            int randomUnitIndex = Random.Range(0, enemies.Count - 1);
            GameObject newBuff = Instantiate(RangedAttack, transform.position, Quaternion.identity);
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, enemies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, enemies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Vector3 previousEnemyPos = enemies[randomUnitIndex].transform.position;
            enemies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff * level;
            enemies[randomUnitIndex].GetComponent<Unit>().health += healthBuff * level;
            if (enemies[randomUnitIndex].GetComponent<Unit>().health <= 0) StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnDie());
            else StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnHurt());
            StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().CollisionJiggle());
            StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().BuffJuice(1));
            enemies.RemoveAt(randomUnitIndex);

            for (int i = 0; i < level; i++)
            {
                if(enemies.Count > 0)
                {
                    buffTimer = 0;
                    randomUnitIndex = Random.Range(0, enemies.Count - 1);
                    while (buffTimer <= buffTime)
                    {
                        newBuff.transform.position = new Vector2(Mathf.Lerp(previousEnemyPos.x, enemies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                            Mathf.Lerp(previousEnemyPos.y, enemies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                        buffTimer += Time.deltaTime;
                        yield return null;
                    }
                    previousEnemyPos = enemies[randomUnitIndex].transform.position;
                    enemies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
                    if (enemies[randomUnitIndex].GetComponent<Unit>().health <= 0) StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnDie());
                    else StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().OnHurt());
                    StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().CollisionJiggle());
                    StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().BuffJuice(1));
                    enemies.RemoveAt(randomUnitIndex);
                }
            }
            Destroy(newBuff);
        }
        yield return StartCoroutine(base.OnStartOfBattle());
    }
}