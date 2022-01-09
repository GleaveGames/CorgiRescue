using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Advisor : Unit
{

    bool jiggled = false;


    //End Turn, drain +1/+1 from adjacent units
    public override IEnumerator OnEndTurn()
    {
        actioning = true;
        for (int x = -1; x <= 1; x++)
        {
            for (int y = -1; y <= 1; y++)
            {
                if (x == 0 && y == 0)
                {
                    continue;
                }
                else
                {
                    StartCoroutine(CheckForUnitAndBuff(x, y));
                }
            }
        }
        actioning = false;
        yield return StartCoroutine(base.OnEndTurn());
    }
    private IEnumerator CheckForUnitAndBuff(int x, int y)
    {
        GameSquare square = null;
       
        Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), playerTiles);
        if (squareCol != null)
        {
            square = squareCol.GetComponent<GameSquare>();
        }
        
        //Give Buff
        if (square != null && square.occupied)
        {
            if (!jiggled) StartCoroutine(Jiggle());
            jiggled = true;
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, square.transform.position, Quaternion.identity);
            sm.PlayHurt();
            square.occupier.GetComponent<Unit>().StartCoroutine(CollisionJiggle());
            square.occupier.GetComponent<Unit>().StartCoroutine(BuffJuice(3));
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(square.transform.position.x, transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(square.transform.position.y, transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(newBuff);
            if (square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().health <= healthBuff*level) square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().health = 1;
            else square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().health -= healthBuff * level;
            if (square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().attack <= attackBuff * level) square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().attack = 1;
            else square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().attack -= attackBuff * level;
            attack += attackBuff*level;
            health += healthBuff*level;

            StartCoroutine(Jiggle());
            StartCoroutine(BuffJuice(3));
        }
        else
        {
            yield return null;
        }
    }

}
