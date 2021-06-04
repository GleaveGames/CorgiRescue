using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UndeadArcher : RangedTroop
{
    public override IEnumerator Attack()
    {
        float timer = 0.2f;
        movepos = closestEnemy.transform.position;
        Vector2 originalPos = transform.position;
        while (timer > 0 && closestEnemy != null)
        {
            timer -= Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, movepos, attackSpeed * Time.deltaTime);
            yield return null;
        }
        GameObject projectile = Instantiate(Projectile, transform.position, Quaternion.identity);
        NetworkServer.Spawn(projectile);
        Vector3 enemypos = projectile.transform.position;
        if (closestEnemy != null)
        {
            projectile.transform.up = closestEnemy.transform.position - transform.position;
            enemypos = closestEnemy.transform.position;
        }
        while (projectile.transform.position != enemypos && closestEnemy != null)
        {
            projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, enemypos, projectileSpeed * Time.deltaTime);
            yield return null;
        }
        Destroy(projectile);
        if (closestEnemy != null)
        {
            Debug.Log("ArrowDamage");
            closestEnemy.GetComponent<CharacterStats>().health -= damage;
            closestEnemy.GetComponent<CharacterStats>().UpdateClientHealth();
            if (closestEnemy.TryGetComponent(out Troop troop)) troop.speed *= 0.6f;
        }
        CheckForEnemies();
        while (new Vector2(transform.position.x, transform.position.y) != originalPos)
        {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, attackSpeed / 2 * Time.deltaTime);
            yield return null;
        }
        moving = false;
        attacking = false;
    }

}
