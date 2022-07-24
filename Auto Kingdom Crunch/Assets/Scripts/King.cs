﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Unit
{
    public override IEnumerator OnStartOfTurn()
    {
        if (playerUnit)
        {
            actioning = true;
            List<GameObject> allies = GetAllies();
            int peasants = 0;
            foreach (GameObject u in allies)
            {
                if (u.name.Contains("Peasant"))
                {
                    peasants++;
                }
            }

            if(peasants > 0)
            {
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
                attack += attackBuff * peasants *level ;
                health += healthBuff * peasants * level;
                FindObjectOfType<GameController>().Gold += peasants;
                StartCoroutine(BuffJuice(3, attackBuff*peasants*level,healthBuff*peasants*level));
                StartCoroutine(Jiggle());
                FindObjectOfType<Shop>().reroll.Play();
            }
        }
        yield return StartCoroutine(base.OnStartOfTurn());
    }

}