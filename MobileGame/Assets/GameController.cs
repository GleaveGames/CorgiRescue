using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject draggingObj;
    [SerializeField]
    LayerMask squares;
    [SerializeField]
    LayerMask enemysquares;
    bool Battling = false;
    

    private void Update()
    {
        if (Battling) return;
        if (draggingObj != null)
        {
            draggingObj.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            
            //stop dragging if mouse up
            if (Input.GetMouseButtonUp(0))
            {
                Collider2D square = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), squares);
                if (square != null && !square.GetComponent<GameSquare>().occupied)
                {
                    draggingObj.transform.position = square.transform.position;
                    square.GetComponent<GameSquare>().occupied = true;
                    square.GetComponent<GameSquare>().occupier = draggingObj;

                    if (!draggingObj.GetComponent<ShopSprite>().beenPlaced) draggingObj.GetComponent<ShopSprite>().Bought();
                    draggingObj = null;
                }
                else
                {
                    //combine
                    if (square!= null && draggingObj.name == square.GetComponent<GameSquare>().occupier.name)
                    {
                        Destroy(draggingObj);
                        square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().Combine();
                    }
                    else
                    {
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj = null;
                    }
                }
            }
        }
    }

    public void BattleTrigger()
    {
        StartCoroutine(Battle());
    }

    public IEnumerator Battle()
    {
        Battling = true;

        //get player Units
        GameObject[] units = new GameObject[transform.childCount - 3];
        for (int i = 3; i < transform.childCount; i++)
        {
            units[i-3] = transform.GetChild(i).gameObject;
        }

        //get enemy units
        for (int i = 0; i < units.Length; i++)
        {
            bool placed = false;
            while(!placed)
            {
                float x = Random.Range(0, 6) - 2.5f;
                int y = Random.Range(1, 4);
                Vector2 spawnPoint = new Vector2(x, y);
                Collider2D square = Physics2D.OverlapPoint(spawnPoint, enemysquares);
                if (square != null && !square.GetComponent<GameSquare>().occupied) { 
                    GameObject newUnit = Instantiate(units[i], spawnPoint, Quaternion.identity);
                    square.GetComponent<GameSquare>().occupied = true;
                    square.GetComponent<GameSquare>().occupier = newUnit;
                    placed = true;
                    newUnit.transform.parent = transform;
                }
                yield return null;
            }
        }
    }

    static GameObject[] InsertionSort(GameObject[] inputArray)
    {
        for (int i = 0; i < inputArray.Length - 1; i++)
        {
            for (int j = i + 1; j > 0; j--)
            {
                if (inputArray[j - 1].GetComponent<Unit>().attack > inputArray[j].GetComponent<Unit>().attack)
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
