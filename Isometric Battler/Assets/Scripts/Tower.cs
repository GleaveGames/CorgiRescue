using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public class Tower : NetworkBehaviour
{
    Coroutine coroutine;
    protected GameObject closestEnemy;
    protected GameManager gm;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected GameObject Projectile;
    protected Transform firepos;
    [SerializeField]
    protected float attackRate;
    [SerializeField]
    protected float projectileSpeed;
    [SerializeField]
    protected int damage;

    private void Start()
    {
        if (!isServer) this.enabled = false;
        gm = FindObjectOfType<GameManager>();
        StartCoroutine(EnemyCheckandAttack());
        firepos = transform.GetChild(0);
    }

    protected virtual IEnumerator EnemyCheckandAttack() 
    {
        yield return new WaitForSeconds(attackRate);
        CheckForEnemies();
        if(closestEnemy != null) 
        {
            if(Vector2.Distance(transform.position, closestEnemy.transform.position) <= range) 
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
                    closestEnemy.GetComponent<CharacterStats>().UpdateClientHealth();

                }
            }
        }
        StartCoroutine(EnemyCheckandAttack());
    }


    protected void CheckForEnemies()
    {
        closestEnemy = null;
        float closestDistance = 99;
        foreach (Team team in gm.teams)
        {
            if (team.color != GetComponent<SpriteRenderer>().color)
            {
                if (closestEnemy == null)
                {
                    if (team.things.Count > 0)
                    {
                        closestEnemy = team.things[0];
                        if(closestEnemy == null) 
                        {
                            return;
                        }
                        closestDistance = Vector2.Distance(transform.position, closestEnemy.transform.position);
                    }
                    else
                    {
                        Debug.Log("No enemies");
                    }
                }
                foreach (GameObject thing in team.things)
                {
                    if (thing != null)
                    {
                        if (Vector2.Distance(transform.position, thing.gameObject.transform.position) < closestDistance)
                        {
                            closestEnemy = thing;
                            closestDistance = Vector2.Distance(transform.position, thing.gameObject.transform.position);
                        }
                    }
                }
            }
        }
    }
}
