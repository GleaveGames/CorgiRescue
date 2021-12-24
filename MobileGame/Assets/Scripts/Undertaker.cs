using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Undertaker : Unit
{
    List<Unit> adjacents;


    public override void Start()
    {
        adjacents = new List<Unit>();
        base.Start();
    }

    public override void Update()
    {
        for(int i = adjacents.Count-1; i >= 0; i--)
        {
            if(adjacents[i].health <= 0)
            {
                //buff
                StartCoroutine(buff());
            }
        }

        adjacents = new List<Unit>();


        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (x == 0 && y == 0) continue;
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), playerTiles);
                if (squareCol == null) squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), enemyTiles);
                GameSquare square = null;
                if (squareCol != null)
                {
                    square = squareCol.GetComponent<GameSquare>();
                }
                if (square != null && square.occupied && square.occupier.GetComponent<Unit>().health > 0)
                {
                    adjacents.Add(square.occupier.GetComponent<Unit>());
                }
            }
        }
        base.Update();
    }



    private IEnumerator buff()
    {
        actioning = true;
        StartCoroutine(Jiggle());
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
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
        StartCoroutine(Jiggle());
        actioning = false;
    }
}
