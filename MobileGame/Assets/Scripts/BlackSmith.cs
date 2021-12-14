using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackSmith : Unit
{
    public override IEnumerator OnEndTurn()
    {
        actioning = true;
        GameSquare square = null;
        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1), playerTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1), enemyTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        if (square != null && square.occupied)
        {
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            StartCoroutine(Jiggle());
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, square.transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, square.transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            square.occupier.GetComponent<Unit>().attack += attackBuff * level;
            StartCoroutine(square.occupier.GetComponent<Unit>().Jiggle());
        }
        yield return StartCoroutine(base.OnEndTurn());
    }
}
