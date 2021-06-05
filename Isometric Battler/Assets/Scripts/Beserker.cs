using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Beserker : Troop
{
    [SerializeField]
    AnimationCurve buildX;
    [SerializeField]
    AnimationCurve buildY;
    [SerializeField]
    AnimationCurve Jump;
    [SerializeField]
    AnimationCurve RingAlpha;
    [SerializeField]
    AnimationCurve RingScale;
    [SerializeField]
    float buildtime;
    [SerializeField]
    float jumptime;
    [SerializeField]
    float jumpHeight;
    [SerializeField]
    Color ringCol;
    [SerializeField]
    float AOErange;
    Transform ring;

    public override void Start()
    {
        base.Start();
        ring = transform.GetChild(0);
    }

    public override IEnumerator Attack()
    {
        Vector2 initialPos = transform.position;
        float counter = 0f;
        Vector3 tempScale = transform.localScale;
        while(counter <= buildtime)
        {
            tempScale.x = buildX.Evaluate(counter / buildtime);
            tempScale.y = buildY.Evaluate(counter / buildtime);
            transform.localScale = tempScale;
            counter += Time.deltaTime;
            yield return null;
        }
        counter = 0f;
        Vector2 tempjump = transform.position;
        float starty = tempjump.y;
        //Jump1
        while (counter <= jumptime) 
        {
            tempjump.y = starty + jumpHeight * Jump.Evaluate(counter / jumptime);
            transform.position = tempjump;
            counter += Time.deltaTime;
            yield return null;
        }

        transform.position = initialPos;
        counter = 0;
        //Jump2
        StartCoroutine(RingAttack());
        while (counter <= jumptime) 
        {
            tempjump.y = starty + jumpHeight * Jump.Evaluate(counter / jumptime);
            transform.position = tempjump;
            counter += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(RingAttack());
        transform.position = initialPos;

        yield return new WaitForSeconds(1);
        moving = false;
        attacking = false;
        
    }

    private IEnumerator RingAttack() 
    {
        float counter = 0f;
        Vector2 initialPos = transform.position;
        Vector2 initialScale = Vector2.zero;
        ring.position = initialPos;
        ring.localScale = initialScale;
        Color temp = ringCol;
        while (counter < jumptime) 
        {
            ring.localScale = new Vector2(RingScale.Evaluate(counter / jumptime) * 2.83f, RingScale.Evaluate(counter / jumptime) * 1.63f);
            temp.a = RingAlpha.Evaluate(counter / jumptime);
            ring.gameObject.GetComponent<SpriteRenderer>().color = temp;
            ring.position = initialPos;
            counter += Time.deltaTime;
            if (isServer) ClientRing(temp) ;
            yield return null;
        }
        List<GameObject> EnemiesInRange = GetEnemiesAOE();
        foreach(GameObject enemy in EnemiesInRange) 
        {
            if (enemy == null) continue;
            enemy.GetComponent<CharacterStats>().health -= damage;
            closestEnemy.GetComponent<CharacterStats>().UpdateClientHealth();
        }
        ring.position = initialPos;
        ring.localScale = initialScale;
        ring.gameObject.GetComponent<SpriteRenderer>().color = ringCol;
    }

    [ClientRpc]
    private void ClientRing(Color temp)
    {
        ring.gameObject.GetComponent<SpriteRenderer>().color = temp;
    }

    List<GameObject> GetEnemiesAOE() 
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach (Team team in gm.teams)
        {
            if (team.color != GetComponent<SpriteRenderer>().color)
            {
                foreach (GameObject thing in team.things)
                {
                    if (thing != null)
                    {
                        if (Vector2.Distance(transform.position, thing.transform.position) < AOErange)
                        {
                            enemies.Add(thing);
                        }
                    }
                }
            }
        }
        return enemies;
    }
}
