using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : Unit
{
    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        List<GameObject> allies = GetAllies();

        int randomUnitIndex = Random.Range(0,allies.Count-1);
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while(buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2*buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
        allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        yield return StartCoroutine(base.OnStartOfBattle());
    }
    
    public override IEnumerator OnDie()
    {
        actioning = true;
        //basic buff
        List<GameObject> allies = GetAllies();
        //Specific to Buff if there is no ally
        if (allies.Count > 0)
        {
            int randomUnitIndex = Random.Range(0, allies.Count - 1);
            float buffTimer = 0;
            GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }

            Destroy(newBuff);
            allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
            allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        }
        
        yield return StartCoroutine(base.OnDie());
    }

    public override IEnumerator OnHurt()
    {
        actioning = true;
        List<GameObject> allies = GetAllies();
        //basic buff
        
        int randomUnitIndex = Random.Range(0, allies.Count - 1);
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
        allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        yield return StartCoroutine(base.OnHurt());
    }
    public override IEnumerator OnBuy()
    {
        List<GameObject> allies = GetAllies();

        int randomUnitIndex = Random.Range(0, allies.Count - 1);
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
        allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        yield return StartCoroutine(base.OnBuy());
    }
    public override IEnumerator Attack()
    {
        attacking = true;
        List<GameObject> allies = GetAllies();

        int randomUnitIndex = Random.Range(0, allies.Count - 1);
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
        allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        yield return StartCoroutine(base.Attack());
    }
    public override IEnumerator OnEndTurn()
    {
        List<GameObject> allies = GetAllies();

        int randomUnitIndex = Random.Range(0, allies.Count - 1);
        float buffTimer = 0;
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[randomUnitIndex].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[randomUnitIndex].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }

        Destroy(newBuff);
        allies[randomUnitIndex].GetComponent<Unit>().attack += attackBuff;
        allies[randomUnitIndex].GetComponent<Unit>().health += healthBuff;
        yield return StartCoroutine(base.OnEndTurn());
    }
}
