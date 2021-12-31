using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : Unit  
{
    [SerializeField]
    Sprite blessing;
    public override IEnumerator OnStartOfTurn()
    {
        //gain +2/+2 if in front of a churchmember
        actioning = true;
        bool gettingBuff = false;
        GameSquare square = null;

        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1.25f), playerTiles);
            if (squareCol != null) square = squareCol.GetComponent<GameSquare>();
        }
        if (square != null && square.occupied && square.occupier.GetComponent<Unit>().quality == 3) gettingBuff = true;

        if (gettingBuff)
        {
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            newBuff.GetComponent<SpriteRenderer>().sprite = blessing;
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            attack += attackBuff * level;
            health += healthBuff * level;
            StartCoroutine(BuffJuice(3));
            StartCoroutine(Jiggle());
        }

        yield return StartCoroutine(base.OnStartOfTurn());
    }
}
