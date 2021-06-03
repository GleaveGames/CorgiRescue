using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Soldier : Troop
{
    public override IEnumerator Attack() 
    {
        movepos = closestEnemy.transform.position;
        Vector2 originalPos = transform.position;
        while (new Vector2(transform.position.x, transform.position.y) != movepos && closestEnemy != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, movepos, attackSpeed * Time.deltaTime);
            yield return null;
        }
        if(closestEnemy != null) { 
            closestEnemy.GetComponent<CharacterStats>().health -= damage;
        }
        base.CheckForEnemies();
        while (new Vector2(transform.position.x, transform.position.y) != originalPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, attackSpeed/2 * Time.deltaTime);
            yield return null;
        }
        moving = false;
        attacking = false;
    }
}
