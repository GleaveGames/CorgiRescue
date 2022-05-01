using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prisoner : Unit
{
    [SerializeField]
    GameObject spawnedPeasant;
    public override IEnumerator OnDie()
    {
        
        //if on his own row, spawn a peasant level 1 peasant when dead;
        actioning = true;
        
        bool noUnits = true;
        /*
        int y = 0;
        for (int x = -1; x <= 1; x++)
        {
            if (x == 0 && y == 0)
            {
                continue;
            }
            else if (CheckFoUnit(x, y))
            {
                noUnits = false;
            }
        }
        */
        yield return StartCoroutine(base.OnDie());

        if (noUnits)
        {

            GameObject pes1 = null;
            Collider2D squareCol = null;
            if (playerUnit) squareCol = Physics2D.OverlapPoint(transform.position, playerTiles);
            else squareCol = Physics2D.OverlapPoint(transform.position, enemyTiles);
            if (squareCol != null)
            {
                GameSquare square = squareCol.GetComponent<GameSquare>();
                square.occupied = false;
                square.occupier = null;
                if (!square.occupied)
                {
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
           
            if (pes1 != null)
            {
                while (pes1.GetComponent<Unit>().actioning) yield return null;
            }
        }
        
    }

    private bool CheckFoUnit(int x, int y)
    {
        bool check = false;
        GameSquare square = null;
        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), playerTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(x * 1.25f, y * 1.25f), enemyTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        if (square != null && square.occupied) check = true;
        return check;
    }
}
