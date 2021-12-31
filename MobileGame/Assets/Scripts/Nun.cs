using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nun : Unit
{
    [SerializeField]
    GameObject spawnedPeasant;
    bool spawned;
    public override IEnumerator OnStartOfBattle()
    {
        //Spawn a 1,1 peasant on either side that dies at the end of battle
        GameObject pes1 = null;
        GameObject pes2 = null;
        
        actioning = true;
        Collider2D squareCol = null;
        if (playerUnit)  squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(1.25f, 0), playerTiles);
        else squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(1.25f, 0), enemyTiles);
        if (squareCol != null)
        {
            GameSquare square = squareCol.GetComponent<GameSquare>();
            if (!square.occupied) {
                pes1 = Instantiate(spawnedPeasant, square.transform.position, Quaternion.identity);
                square.occupied = true;
                square.occupier = pes1;
                pes1.GetComponent<Unit>().level = level;
                pes1.GetComponent<Unit>().playerUnit = playerUnit;
                pes1.GetComponent<Unit>().temperary = true;
                pes1.transform.parent = transform.parent;
                yield return new WaitForEndOfFrame();
                StartCoroutine(pes1.GetComponent<Unit>().OnStartOfBattle());
            }
        }
        squareCol = null;
        if (playerUnit)  squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(-1.25f, 0), playerTiles);
        else squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(-1.25f, 0), enemyTiles);
        if (squareCol != null)
        {
            GameSquare square = squareCol.GetComponent<GameSquare>();
            if (!square.occupied)
            {
                pes2 = Instantiate(spawnedPeasant, square.transform.position, Quaternion.identity);
                square.occupied = true;
                square.occupier = pes2;
                pes2.GetComponent<Unit>().level = level;
                pes2.GetComponent<Unit>().playerUnit = playerUnit;
                pes2.GetComponent<Unit>().temperary = true;
                pes2.transform.parent = transform.parent;
                yield return new WaitForEndOfFrame();
                StartCoroutine(pes2.GetComponent<Unit>().OnStartOfBattle());
            }
        }
        StartCoroutine(Jiggle());
        if(pes1 != null || pes2 != null)
        {
            if(pes1 != null)
            {
                while (pes1.GetComponent<Unit>().actioning) yield return null;
            }
            if(pes2 != null)
            {
                while (pes2.GetComponent<Unit>().actioning) yield return null;
            }
        }

        yield return StartCoroutine(base.OnStartOfBattle());
    }
}
