using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apprentice : Unit
{
    //copy the unit to right
    public override IEnumerator OnEndTurn()
    {
        actioning = true;
        Collider2D col = Physics2D.OverlapPoint(transform.position - new Vector3(1.25f, 0, 0),playerTiles);
        if (!playerUnit) col = Physics2D.OverlapPoint(transform.position - new Vector3(1.25f, 0, 0), enemyTiles);
        GameSquare square = col.GetComponent<GameSquare>();
        attackPreBattle = attack;
        healthPreBattle = health;
        levelPreBattle = level;
        expPreBattle = exp;

        if (square.occupier != null)
        {
            //the old switcheroo
            Instantiate(cloudParticles, transform.position, Quaternion.identity);
            GameObject clone = Instantiate(square.occupier, transform.position, Quaternion.identity);
            clone.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            clone.GetComponent<Unit>().health = health;
            clone.GetComponent<Unit>().attack = attack;
            clone.GetComponent<Unit>().level = level;
            clone.GetComponent<Unit>().exp = 1;
            clone.GetComponent<Unit>().playerUnit = playerUnit;
            Collider2D col2 = Physics2D.OverlapPoint(transform.position, playerTiles);
            if (!playerUnit) col2 = Physics2D.OverlapPoint(transform.position, enemyTiles);
            GameSquare square2 = col2.GetComponent<GameSquare>();
            square2.occupier = clone;
            square2.occupied = true;
            clone.GetComponent<Unit>().temperary = true;
            clone.transform.parent = transform.parent;
            yield return new WaitForEndOfFrame();
            StartCoroutine(clone.GetComponent<Unit>().OnEndTurn());

            health = 0;
        }

        yield return StartCoroutine(base.OnEndTurn());
    }


}
