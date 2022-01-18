using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Executioner : Unit
{
    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        GameSquare square = null;
        Collider2D squareCol = null;
        if (playerUnit) squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1), playerTiles);
        else squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1), enemyTiles);
        if (squareCol != null) square = squareCol.GetComponent<GameSquare>();
        if (square != null && square.occupied)
        {
            yield return new WaitForSeconds(0.1f);
            while(square.occupier!=null && square.occupier.GetComponent<Unit>().actioning)
            {
                yield return null;
            }
            yield return new WaitForSeconds(0.1f);
            if (square.occupier == null)
            {
                yield return StartCoroutine(base.OnStartOfBattle());
                yield break;
            }
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            StartCoroutine(Jiggle());
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(square.transform.position.x, transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(square.transform.position.y, transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            if (square.occupier != null)
            {
                health += Mathf.RoundToInt((level / 2f) * square.occupier.GetComponent<Unit>().health);
                attack += Mathf.RoundToInt((level / 2f) * square.occupier.GetComponent<Unit>().attack);
                StartCoroutine(square.occupier.GetComponent<Unit>().OnDie());
                square.occupier.GetComponent<Unit>().health = 0;
                square.occupier.GetComponent<Unit>().attack = 0;
                StartCoroutine(BuffJuice(3));
                StartCoroutine(Jiggle());
                while (square.occupier!=null && square.occupier.GetComponent<Unit>().actioning) yield return null;
            }
        }
        yield return StartCoroutine(base.OnStartOfBattle());
    }
}
