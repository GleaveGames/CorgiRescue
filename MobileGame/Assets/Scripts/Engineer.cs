using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engineer : Unit
{
    [SerializeField]
    Sprite bomb;
    public override IEnumerator OnDie()
    {
        actioning = true;
        //bomb back row
        List<GameObject> victims = new List<GameObject>();
        float y = 0;
        LayerMask layer = playerTiles;
        if (playerUnit) 
        {
            y = 3.75f;
            layer = enemyTiles;
        }
        else {
            y = -2.5f;
        }

        for(int x = 0; x < 6; x++)
        {
            Collider2D col = Physics2D.OverlapPoint(new Vector2(1.25f * x - 2.5f, y), layer);
            GameSquare square = col.GetComponent<GameSquare>();
            if (square.occupied)
            {
                victims.Add(col.GetComponent<GameSquare>().occupier);
            }
        }

        foreach(GameObject go in victims)
        {
            float buffTimer = 0;
            StartCoroutine(Jiggle());
            GameObject newBuff = Instantiate(RangedAttack, transform.position, Quaternion.identity);
            newBuff.GetComponent<SpriteRenderer>().sprite = bomb;
            newBuff.GetComponent<Buff>().good = false;
            while (buffTimer <= buffTime)
            {
                if (go == null) yield break;
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, go.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, go.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            go.GetComponent<Unit>().health += healthBuff * level;
            if (go.GetComponent<Unit>().health <= 0) StartCoroutine(go.GetComponent<Unit>().OnDie());
            else StartCoroutine(go.GetComponent<Unit>().OnHurt());
            StartCoroutine(go.GetComponent<Unit>().CollisionJiggle());
            ShowDamage(-healthBuff * level, go.transform.position);

            while (go != null && go.GetComponent<Unit>().actioning) yield return null;
        }
        yield return StartCoroutine(base.OnDie());
    }
}
