using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : Unit
{
    public override IEnumerator Attack()
    {
        attacking = true;

        //get nearest enemy 

        for (int y = 1; y < 4; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Collider2D square = null;
                Vector2 spawnPoint = Vector2.zero;
                if (playerUnit)
                {
                    spawnPoint = new Vector2(x * 1.25f - 2.5f, y * 1.25f);
                    square = Physics2D.OverlapPoint(spawnPoint, enemyTiles);
                }
                else
                {
                    spawnPoint = new Vector2(x * 1.25f - 2.5f, 1.25f - y * 1.25f);
                    square = Physics2D.OverlapPoint(spawnPoint, playerTiles);
                }
                if (square.GetComponent<GameSquare>().occupied)
                {
                    //Enemy here and attack

                    float timer = 0;
                    initPos = transform.position;
                    Unit enemyUnit = square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                    bool particlesGoneOff = false;
                    GameObject particle = null;
                    Vector2 particleSpawnPos = new Vector2(0, 0);
                    Vector3 initParticleScale = new Vector3(0.2f, 0.2f, 1);
                    while (timer < attackTime)
                    {


                        if (timer > attackTime / 2 && !particlesGoneOff)
                        {
                            particlesGoneOff = true;
                            particle = Instantiate(collisionParticle, transform.position, Quaternion.identity);
                            particleSpawnPos = (transform.position + enemyUnit.transform.position) / 2;
                            particle.transform.up = Vector2.Perpendicular(spawnPoint - initPos);
                            StartCoroutine(CollisionJiggle());
                            if (playerUnit) gc.playHit();
                        }
                        else if (timer > attackTime / 2)
                        {
                            particle.transform.position = Vector2.Lerp(particleSpawnPos, particleSpawnPos + Vector2.Perpendicular(spawnPoint - initPos) / 5, (timer - attackTime / 2) / (attackTime / 2));
                            particle.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, colorInvisible, (timer - attackTime / 2) / (attackTime / 2));
                            particle.transform.localScale = Vector3.Lerp(initParticleScale, new Vector3(1, 1, 1), (timer - attackTime / 2) / (attackTime / 2));
                        }

                        transform.position = Vector2.Lerp(initPos, spawnPoint, attackCurve.Evaluate(timer / attackTime));
                        timer += Time.deltaTime;
                        yield return null;
                    }
                    Destroy(particle);
                    Collider2D colLeft = null;
                    Collider2D colRight = null;
                    Unit left = null;
                    Unit right = null;

                    if (playerUnit) {
                        colLeft = Physics2D.OverlapPoint(square.transform.position - new Vector3(1.25f, 0, 0), enemyTiles);
                        colRight = Physics2D.OverlapPoint(square.transform.position + new Vector3(1.25f, 0, 0), enemyTiles);
                    }
                    else
                    {
                        colLeft = Physics2D.OverlapPoint(square.transform.position - new Vector3(1.25f, 0, 0), playerTiles);
                        colRight = Physics2D.OverlapPoint(square.transform.position + new Vector3(1.25f, 0, 0), playerTiles);
                    }
                    if (colLeft != null && colLeft.GetComponent<GameSquare>().occupied) left = colLeft.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                    if (colRight != null && colRight.GetComponent<GameSquare>().occupied) right = colRight.GetComponent<GameSquare>().occupier.GetComponent<Unit>();

                    

                    // Code for Damaging a unit
                    enemyUnit.health -= attack;
                    if (enemyUnit.health > 0)
                    {
                        StartCoroutine(enemyUnit.OnHurt());
                        while (enemyUnit.actioning)
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        StartCoroutine(enemyUnit.OnDie());
                        while (enemyUnit.actioning)
                        {
                            yield return null;
                        }
                    }

                    if (left != null)
                    {
                        left.health -= attack;
                        left.StartCoroutine(CollisionJiggle());
                        if (left.health > 0)
                        {
                            StartCoroutine(left.OnHurt());
                            while (left.actioning)
                            {
                                yield return null;
                            }
                        }
                        else
                        {
                            StartCoroutine(left.OnDie());
                            while (left.actioning)
                            {
                                yield return null;
                            }
                        }
                    }
                    if (right != null)
                    {
                        right.health -= attack;
                        right.StartCoroutine(CollisionJiggle());
                        if (right.health > 0)
                        {
                            StartCoroutine(right.OnHurt());
                            while (right.actioning)
                            {
                                yield return null;
                            }
                        }
                        else
                        {
                            StartCoroutine(right.OnDie());
                            while (right.actioning)
                            {
                                yield return null;
                            }
                        }
                    }

                    goto end;
                }
            }
        }
        end:
            yield return new WaitForSeconds(1);
            attacking = false;
    }
}
