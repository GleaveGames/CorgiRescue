using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monk : Unit
{
    [SerializeField]
    Sprite sprite;
    //On Attack Deal 1 damage to units in column
    public override IEnumerator OnAttack()
    {
        actioning = true;
        List<GameObject> units = GetUnitsInRow();
        StartCoroutine(Jiggle());
        foreach (GameObject u in units)
        {
            StartCoroutine(BuffObj(u));
            yield return new WaitForSeconds(0.2f);
        }
        yield return new WaitForSeconds(4*buffTime + units.Count * 0.2f);
        
        yield return StartCoroutine(base.OnAttack());
    }
    private IEnumerator BuffObj(GameObject u)
    {
        GameObject newBuff = Instantiate(RangedAttack, transform.position, Quaternion.identity);
        newBuff.GetComponent<Buff>().good = false;
        float buffTimer = 0;
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, u.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
            Mathf.Lerp(transform.position.y, u.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }
        u.GetComponent<Unit>().health += healthBuff * level;
        ShowDamage(-healthBuff * level, u.transform.position);
        if (u.GetComponent<Unit>().health <= 0) StartCoroutine(u.GetComponent<Unit>().OnDie());
        else StartCoroutine(u.GetComponent<Unit>().OnHurt());
        Destroy(newBuff);
        u.GetComponent<Unit>().CollisionJiggle();
    }

    private List<GameObject> GetUnitsInRow()
    {
        List<GameObject> result = new List<GameObject>();
        for(int y = -5; y <= 5; y++)
        {
            Collider2D col = Physics2D.OverlapPoint(transform.position + new Vector3(0, y * 1.25f, 0),playerTiles);
            if(col == null) col = Physics2D.OverlapPoint(transform.position + new Vector3(0, y * 1.25f, 0), enemyTiles);
            if (col!=null && col.GetComponent<GameSquare>().occupier != null) result.Add(col.GetComponent<GameSquare>().occupier);
        }
        return result;
    }

}
