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
    public bool Battling = false;

    public List<GameObject> playerUnits;
    public List<GameObject> enemyUnits;
    public List<GameObject> allUnits;
    public int lives = 6;
    GameObject livesParent;


    private void Start()
    {
        livesParent = GameObject.Find("Lives");
        ResetLives();
    }

    private void ResetLives()
    {
        for (int i = livesParent.transform.childCount - 1; i >= 0; i--)
        {
            if (lives <= i) livesParent.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

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
                        square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().Combine();
                        if (!draggingObj.GetComponent<ShopSprite>().beenPlaced) square.GetComponent<GameSquare>().occupier.GetComponent<ShopSprite>().Bought();

                        Destroy(draggingObj);

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

        GetPlayerUnits();

        foreach(GameObject u in playerUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnEndTurn());
            while (u.GetComponent<Unit>().actioning)
            {
                yield return null;
            }
        }

        //get enemy units
        for (int i = 0; i < playerUnits.Count; i++)
        {
            bool placed = false;
            while(!placed)
            {
                float x = Random.Range(0, 6) - 2.5f;
                int y = Random.Range(1, 4);
                Vector2 spawnPoint = new Vector2(x, y);
                Collider2D square = Physics2D.OverlapPoint(spawnPoint, enemysquares);
                if (square != null && !square.GetComponent<GameSquare>().occupied) { 
                    GameObject newUnit = Instantiate(playerUnits[i], spawnPoint, Quaternion.identity);

                    //Randomify enemy attack and defense;
                    newUnit.GetComponent<Unit>().attack += Random.Range(0, 5);
                    newUnit.GetComponent<Unit>().health += Random.Range(0, 10);


                    newUnit.GetComponent<Unit>().playerUnit = false;
                    square.GetComponent<GameSquare>().occupied = true;
                    square.GetComponent<GameSquare>().occupier = newUnit;
                    placed = true;
                    newUnit.transform.parent = transform;
                    enemyUnits.Add(newUnit);
                    allUnits.Add(newUnit);
                }
                yield return null;
            }
        }

        foreach(GameObject u in playerUnits)
        {
            u.GetComponent<Unit>().healthPreBattle = u.GetComponent<Unit>().health;
            u.GetComponent<Unit>().attackPreBattle = u.GetComponent<Unit>().attack;
        }

        allUnits = InsertionSort(allUnits);

        foreach(GameObject u in allUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnStartOfBattle());
            while (u.GetComponent<Unit>().actioning)
            {
                yield return null;
            }
        }


        while (Battling)
        {
            foreach (GameObject u in allUnits)
            {
                if (u.GetComponent<Unit>().health > 0)
                {
                    StartCoroutine(u.GetComponent<Unit>().Attack());
                    while (u.GetComponent<Unit>().attacking)
                    {
                        yield return null;
                    }
                }
            }
            if (enemyUnits.Count == 0)
            {
                Battling = false;
                Debug.Log("you win");
            }
            else if (playerUnits.Count == 0) { 
                Battling = false;
                lives -= 1;
                ResetLives();
                Debug.Log("You LOSE");
            }
            yield return null;
        }

        foreach(GameObject u in enemyUnits)
        {
            Collider2D square = Physics2D.OverlapPoint(u.transform.position, enemysquares);
            square.GetComponent<GameSquare>().occupied = false;
            square.GetComponent<GameSquare>().occupier = null;
            Destroy(u);
        }
    }



    private List<GameObject> InsertionSort(List<GameObject> inputArray)
    {
        if (inputArray.Count < 2) return inputArray;
        for (int i = 0; i < inputArray.Count - 1; i++)
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

    public void GetPlayerUnits()
    {
        //get player Units
        playerUnits = new List<GameObject>();
        //make an array more units and enemy units
        allUnits = new List<GameObject>();
        enemyUnits = new List<GameObject>();

        for (int i = 3; i < transform.childCount; i++)
        {
            playerUnits.Add(transform.GetChild(i).gameObject);
            allUnits.Add(transform.GetChild(i).gameObject);
        }
    }
}
