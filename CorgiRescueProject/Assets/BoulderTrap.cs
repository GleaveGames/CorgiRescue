using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : TrapRoom
{
    [SerializeField]
    GameObject boulder;
    [SerializeField]
    GameObject jade;
    [SerializeField]
    float boulderPower;
    [SerializeField]
    AnimationCurve boulderSpeed;
    [SerializeField]
    float boulderMoveLength;

    protected override void Cause()
    {
        if (jade.transform.parent != transform) triggered = true;
    }

    protected override void Effect()
    {
        StartCoroutine(BoulderMove());
    }

    private IEnumerator BoulderMove()
    {
        boulder.GetComponent<Miner>().canMine = true;
        boulder.GetComponent<DamagesPlayer>().canHurt = true;
        boulder.GetComponent<Rigidbody2D>().isKinematic = false;
        float timer = 0;
        while(timer < boulderMoveLength)
        {
            boulder.GetComponent<Rigidbody2D>().velocity = Vector2.left*boulderSpeed.Evaluate(timer/boulderMoveLength) * boulderPower;
            timer += Time.deltaTime;
            yield return null;
        }
    }
}
