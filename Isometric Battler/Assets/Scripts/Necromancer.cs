using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Necromancer : RangedTroop
{
    [SerializeField]
    float timeBetweenSpawns;
    [SerializeField]
    public int spawnAmount;
    [SerializeField]
    AnimationCurve SquirtX;
    [SerializeField]
    AnimationCurve SquirtY;
    [SerializeField]
    GameObject undeadSoldier;

    public override IEnumerator Attack()
    {
        float counter = 0;
        Vector3 scale = transform.localScale;
        Vector3 scaleinit = transform.localScale;
        Vector3 spawnpos = transform.position;
        for (int i = 0; i < spawnAmount; i++) 
        {
            while(counter < timeBetweenSpawns) 
            {
                scale.x = SquirtX.Evaluate(counter / timeBetweenSpawns);
                scale.y = SquirtY.Evaluate(counter / timeBetweenSpawns);
                transform.localScale = scale;
                //JUICE ANIM
                counter += Time.deltaTime;
                yield return null;
            }
            if(isServer) gm.SpawnTroop(undeadSoldier, this.gameObject, spawnpos);
            transform.localScale = scaleinit;
            counter = 0;
        }
        moving = true;
        StartCoroutine(Move());
        while (moving) 
        {
            yield return null;
        }
        moving = true;
        StartCoroutine(Move());
        while (moving) 
        {
            yield return null;
        }
        attacking = false;
    }
}
