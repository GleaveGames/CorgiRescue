using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class UndeadTower : Tower
{
    protected override IEnumerator EnemyCheckandAttack() 
    {
        yield return new WaitForSeconds(attackRate);
        CheckForEnemies();
        if (closestEnemy != null)
        {
            if (Vector2.Distance(transform.position, closestEnemy.transform.position) <= range)
            {
                //Attack;
                GameObject projectile = Instantiate(Projectile, firepos.position, Quaternion.identity);
                NetworkServer.Spawn(projectile);
                projectile.transform.up = closestEnemy.transform.position - firepos.transform.position;
                Vector3 enemypos = closestEnemy.transform.position;
                while (projectile.transform.position != enemypos && closestEnemy != null)
                {
                    projectile.transform.position = Vector2.MoveTowards(projectile.transform.position, enemypos, projectileSpeed * Time.deltaTime);
                    yield return null;
                }
                Destroy(projectile);
                if (closestEnemy != null)
                {
                    closestEnemy.GetComponent<CharacterStats>().health -= damage;
                    if (closestEnemy.TryGetComponent(out Troop troop)) troop.speed *= 0.6f;
                }
            }
        }
        StartCoroutine(EnemyCheckandAttack());
    } 
}
