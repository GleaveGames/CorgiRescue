using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

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
    public int wins = 6;
    public int round = 0;
    public int unitNumber;
    [SerializeField]
    Text livesText;
    [SerializeField]
    Text unitText;
    [SerializeField]
    Text goldText;
    [SerializeField]
    public int Gold;
    [SerializeField]
    Text Round;
    [SerializeField]
    Text Wins;
    [SerializeField]
    AnimationCurve goldJuiceX;
    [SerializeField]
    Transform Clouds;


    [Header("Curves and Things")]
    [SerializeField]
    public AnimationCurve JiggleX;
    [SerializeField]
    public AnimationCurve JiggleY;
    public float jiggleTime;

    public TextAsset database;
    Transform CameraTrans;
    private void Start()
    {
        ResetStats();
        CameraTrans = FindObjectOfType<Camera>().transform;
    }

    private void ResetStats()
    {
        livesText.text = lives.ToString();
        Round.text = (round+1).ToString();
        Wins.text = wins.ToString();
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
                //check if been bought yet
                if(draggingObj.GetComponent<ShopSprite>().beenPlaced)
                {
                    if (square != null && square.name == "Sell")
                    {
                        Gold += draggingObj.GetComponent<Unit>().level;
                        StartCoroutine(draggingObj.GetComponent<Unit>().OnSell());
                        Destroy(draggingObj);
                    }
                    else if (square != null && square.name != "Freeze" && !square.GetComponent<GameSquare>().occupied)
                    {
                        //  MOVE THE SQUARE 
                        draggingObj.transform.position = square.transform.position;
                        square.GetComponent<GameSquare>().occupied = true;
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                    }
                    else if (square != null && square.GetComponent<GameSquare>().occupier != null && draggingObj.name == square.GetComponent<GameSquare>().occupier.name && square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().level != 3)
                    {
                        //COMBINE 
                        square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().Combine();
                        Destroy(draggingObj);
                    }
                    else
                    {
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        Collider2D oldsquare = Physics2D.OverlapPoint(draggingObj.GetComponent<ShopSprite>().origin, squares);
                        oldsquare.GetComponent<GameSquare>().occupied = true;
                        oldsquare.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                    }
                }
                else
                {
                    if (square != null && !draggingObj.GetComponent<ShopSprite>().beenPlaced && square.name == "Freeze" && !square.GetComponent<GameSquare>().occupied)
                    {
                        square.GetComponent<GameSquare>().occupied = true;
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.transform.parent = square.transform;
                        draggingObj.transform.position = square.transform.position;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                    }
                    else if (Gold < 3)
                    {
                        StartCoroutine(GoldJuice());
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                    }
                    else if (square != null && square.name == "Sell")
                    {
                        Gold += draggingObj.GetComponent<Unit>().level;
                        StartCoroutine(draggingObj.GetComponent<Unit>().OnSell());
                        Destroy(draggingObj);
                    }
                    else if (square != null && !square.GetComponent<GameSquare>().occupied && unitNumber < 6)
                    {
                        //Buy onto new Square
                        draggingObj.transform.position = square.transform.position;
                        square.GetComponent<GameSquare>().occupied = true;
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.GetComponent<ShopSprite>().Bought();
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                        Gold -= 3;
                    }
                    else if (square != null && square.GetComponent<GameSquare>().occupier != null && draggingObj.name == square.GetComponent<GameSquare>().occupier.name && square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().level != 3)
                    {
                        //combine 
                        square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().Combine();
                        square.GetComponent<GameSquare>().occupier.GetComponent<ShopSprite>().Bought();
                        Destroy(draggingObj);
                        Gold -= 3;
                    }
                    else
                    {
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                    }
                }
            }
        }
        unitNumber = transform.childCount - 3;
        unitText.text = unitNumber.ToString() + " / 6";
        goldText.text = Gold.ToString();
    }

    public IEnumerator GoldJuice()
    {
        float timer = 0;
        Vector2 goldInit = goldText.transform.position;
        while(timer < 0.6)
        {
            goldText.transform.position = new Vector2(goldInit.x + goldJuiceX.Evaluate(timer), goldInit.y);
            timer += Time.deltaTime;
            yield return null;
        }
        goldText.transform.position = goldInit;
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


        foreach (GameObject u in playerUnits)
        {
            u.GetComponent<Unit>().healthPreBattle = u.GetComponent<Unit>().health;
            u.GetComponent<Unit>().attackPreBattle = u.GetComponent<Unit>().attack;
        }


        //Get Enemy Info

        string databasetext = database.text;
        string[] rounds = databasetext.Split('-');
        string[] lines = rounds[round].Split('\n');
        int choice = Random.Range(1, lines.Length-2);
        string[] sections = lines[choice].Split('[');


        //Spawn Enemies

        int i = 0;
        int characterSelect = 0;
        string allCharacters = null;
        foreach(GameObject u in transform.GetChild(1).GetComponent<Shop>().Units)
        {
            allCharacters += u.GetComponent<Unit>().symbol;
        }
        for (int y = 3; y > 0; y--)
        {
            for (int x = 0; x < 6; x++)
            {
                for(int z = 0; z < allCharacters.Length; z++)
                {
                    if (sections[1][i] == allCharacters[z])
                    {
                        characterSelect++;
                        GameObject newUnit = Instantiate(transform.GetChild(1).GetComponent<Shop>().Units[z], new Vector2(1.25f*x-2.5f,y*1.25f), Quaternion.identity);
                        newUnit.GetComponent<Unit>().playerUnit = false;
                        transform.GetChild(0).GetChild(i).GetComponent<GameSquare>().occupied = true;
                        transform.GetChild(0).GetChild(i).GetComponent<GameSquare>().occupier = newUnit;
                        newUnit.transform.parent = transform;
                        enemyUnits.Add(newUnit);
                        allUnits.Add(newUnit);
                        //set Stats
                        string[] enemyStats = sections[characterSelect+1].Split(',');
                        newUnit.GetComponent<Unit>().level = int.Parse(enemyStats[0]);
                        newUnit.GetComponent<Unit>().attack = int.Parse(enemyStats[1]);
                        newUnit.GetComponent<Unit>().health = int.Parse(enemyStats[2].Substring(0, enemyStats[2].Length - 1));
                    }
                }
                i++;
            }
        }

        float timer = 0;
        while(timer < 2)
        {
            Clouds.transform.position = new Vector3(Clouds.transform.position.x, Mathf.Lerp(4, 12, timer / 2), 0);
            CameraTrans.position = new Vector3(CameraTrans.position.x, Mathf.Lerp(-2.5f, 0.5f, timer / 2), -10);
            timer += Time.deltaTime;
            yield return null;
        }

        
        allUnits = InsertionSort(allUnits);
        

        foreach (GameObject u in allUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnStartOfBattle());
            while (u.GetComponent<Unit>().actioning)
            {
                yield return null;
            }
        }

        while (Battling)
        {
            //get frontmost player unit
            Unit frontMostPlayerUnit = GetFrontmostPlayerUnit();
            Unit frontMostEnemyUnit = GetFrontmostEnemyUnit();
            if (frontMostPlayerUnit != null)
            {
                StartCoroutine(frontMostPlayerUnit.OnAttack());
                StartCoroutine(frontMostEnemyUnit.OnAttack());
                while (frontMostPlayerUnit.actioning || frontMostEnemyUnit.actioning)
                {
                    yield return null;
                }
                StartCoroutine(frontMostPlayerUnit.Attack());
                StartCoroutine(frontMostEnemyUnit.Attack());
                while (frontMostPlayerUnit.attacking || frontMostEnemyUnit.attacking)
                {
                    yield return null;
                }
            }
            if (enemyUnits.Count == 0)
            {
                Battling = false;
                Debug.Log("you win");
                wins++;
            }
            else if (playerUnits.Count == 0) { 
                Battling = false;
                lives -= 1;
                Debug.Log("You LOSE");
            }
            yield return null;
        }

        yield return new WaitForEndOfFrame();

        timer = 0;
        while (timer < 2)
        {
            Clouds.transform.position = new Vector3(Clouds.transform.position.x, Mathf.Lerp(12, 4, timer / 2), 0);
            CameraTrans.position = new Vector3(CameraTrans.position.x, Mathf.Lerp(0.5f, -2.5f, timer / 2), -10);
            timer += Time.deltaTime;
            yield return null;
        }

        foreach (GameObject u in enemyUnits)
        {
            Collider2D square = Physics2D.OverlapPoint(u.transform.position, enemysquares);
            square.GetComponent<GameSquare>().occupied = false;
            square.GetComponent<GameSquare>().occupier = null;
            Destroy(u);
        }
        round++;
        ResetStats();
        Gold = 11;
        FindObjectOfType<Shop>().ReRoll();

        GetPlayerUnits();
        foreach (GameObject u in playerUnits)
        {
            if (!u.GetComponent<Unit>().dead)
            {
                Instantiate(u.GetComponent<Unit>().cloudParticles, u.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
            u.GetComponent<Unit>().health = u.GetComponent<Unit>().healthPreBattle;
            u.GetComponent<Unit>().attack = u.GetComponent<Unit>().attackPreBattle;
            u.GetComponent<Unit>().dead = false;
        }

        foreach (GameObject u in playerUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnStartOfTurn());
            while (u.GetComponent<Unit>().actioning) yield return null;
        }
        
    }

    private Unit GetFrontmostPlayerUnit()
    {
        Unit playerUnit = null;
        for (int y = 1; y < 4; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Collider2D square = null;
                Vector2 spawnPoint = Vector2.zero;
                spawnPoint = new Vector2(1.25f*x - 2.5f, 1.25f-1.25f*y);
                square = Physics2D.OverlapPoint(spawnPoint, squares);
                if (square.GetComponent<GameSquare>().occupied)
                {
                    return square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                }
            }
        }
        return playerUnit;
    }
    private Unit GetFrontmostEnemyUnit()
    {
        Unit playerUnit = null;
        for (int y = 1; y < 4; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Collider2D square = null;
                Vector2 spawnPoint = new Vector2(1.25f * x - 2.5f, y * 1.25f);
                square = Physics2D.OverlapPoint(spawnPoint, enemysquares);
                if (square.GetComponent<GameSquare>().occupied)
                {
                    return square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                }
            }
        }
        return playerUnit;
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

            if (transform.GetChild(i).GetComponent<Unit>().playerUnit) playerUnits.Add(transform.GetChild(i).gameObject);
            else enemyUnits.Add(transform.GetChild(i).gameObject);
            allUnits.Add(transform.GetChild(i).gameObject);
        }
    }

}
