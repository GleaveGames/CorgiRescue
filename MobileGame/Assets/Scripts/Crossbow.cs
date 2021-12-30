﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crossbow : Unit
{
    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        if ((playerUnit && transform.position.y == -2.5) || (!playerUnit && transform.position.y == 3.75))
        {
            List<GameObject> enemies = GetEnemies();
            float buffTimer = 0;
            if (enemies.Count > 0)
            {
                StartCoroutine(Jiggle());
                int randomUnitIndex = Random.Range(0, enemies.Count - 1);
                GameObject newBuff = Instantiate(RangedAttack, transform.position, Quaternion.identity);
                while (buffTimer <= buffTime)
                {
                    newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, enemies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                        Mathf.Lerp(transform.position.y, enemies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                    buffTimer += Time.deltaTime;
                    yield return null;
                }

                Destroy(newBuff);
                enemies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff * level;
                enemies[randomUnitIndex].GetComponent<Unit>().health += healthBuff * level;
                StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().CollisionJiggle());
                StartCoroutine(enemies[randomUnitIndex].GetComponent<Unit>().BuffJuice(1));
            }
        }
        yield return StartCoroutine(base.OnStartOfBattle());
    }
}