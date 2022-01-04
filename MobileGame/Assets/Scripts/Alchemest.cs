using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alchemest : Unit
{
    [SerializeField]
    Sprite potion;
    [SerializeField]
    Sprite badPotion;

    public override IEnumerator OnStartOfBattle()
    {
        actioning = true;
        List<GameObject> allies = GetAllies();
        List<GameObject> enemies = GetEnemies();
        allies.Remove(gameObject);
        allies = InsertionSort(allies);
        enemies = InsertionSort(enemies);

        float buffTimer = 0;
        StartCoroutine(Jiggle());
        GameObject newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
        newBuff.GetComponent<SpriteRenderer>().sprite = potion;
        while (buffTimer <= buffTime)
        {
            newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, allies[0].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                Mathf.Lerp(transform.position.y, allies[0].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
            buffTimer += Time.deltaTime;
            yield return null;
        }
        Destroy(newBuff);
        if (level == 1 || level == 2) allies[0].GetComponent<Unit>().attack += attackBuff;
        else allies[0].GetComponent<Unit>().attack += attackBuff * 2;
        if (level == 1 || level == 2) allies[0].GetComponent<Unit>().health += healthBuff;
        else allies[0].GetComponent<Unit>().attack += attackBuff * 2; StartCoroutine(allies[0].GetComponent<Unit>().BuffJuice(3));
        StartCoroutine(allies[0].GetComponent<Unit>().Jiggle());
        
        
        if(level == 2 || level == 3)
        {
            StartCoroutine(Jiggle());
            newBuff = Instantiate(Buff, transform.position, Quaternion.identity);
            newBuff.GetComponent<SpriteRenderer>().sprite = badPotion;
            while (buffTimer <= buffTime)
            {
                newBuff.transform.position = new Vector2(Mathf.Lerp(transform.position.x, enemies[enemies.Count - 1].transform.position.x, buffX.Evaluate(buffTimer / buffTime)),
                    Mathf.Lerp(transform.position.y, enemies[enemies.Count - 1].transform.position.y, buffTimer / buffTime) + 2 * buffY.Evaluate(buffTimer / buffTime));
                buffTimer += Time.deltaTime;
                yield return null;
            }
            Destroy(newBuff);
            if (level == 1 || level == 2) enemies[enemies.Count-1].GetComponent<Unit>().attack -= attackBuff;
            else enemies[enemies.Count - 1].GetComponent<Unit>().attack -= attackBuff * 2;
            if (level == 1 || level == 2) enemies[enemies.Count - 1].GetComponent<Unit>().health -= healthBuff;
            if (enemies[enemies.Count - 1].GetComponent<Unit>().health < 0) enemies[enemies.Count - 1].GetComponent<Unit>().health = 0;
            else enemies[enemies.Count - 1].GetComponent<Unit>().attack -= attackBuff * 2; 
            StartCoroutine(enemies[enemies.Count - 1].GetComponent<Unit>().BuffJuice(3));
            StartCoroutine(enemies[enemies.Count - 1].GetComponent<Unit>().Jiggle());
            if (enemies[enemies.Count - 1].GetComponent<Unit>().health <= 0) StartCoroutine(enemies[enemies.Count - 1].GetComponent<Unit>().OnDie());
        }

        yield return StartCoroutine(base.OnStartOfBattle());
    }


    private List<GameObject> InsertionSort(List<GameObject> inputArray)
    {
        if (inputArray.Count < 2) return inputArray;
        for (int i = 0; i < inputArray.Count - 1; i++)
        {
            for (int j = i + 1; j > 0; j--)
            {
                if (inputArray[j - 1].GetComponent<Unit>().health > inputArray[j].GetComponent<Unit>().health)
                {
                    GameObject temp = inputArray[j - 1];
                    inputArray[j - 1] = inputArray[j];
                    inputArray[j] = temp;
                }
            }
        }
        return inputArray;
    }
}
