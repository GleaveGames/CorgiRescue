using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    bool LOCALTESTING;
    [SerializeField]
    string testEnemy;
    public string databasetext;

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
    Button BattleButton;
    [SerializeField]
    Text livesText;    
    [SerializeField]
    Text livesText2;
    [SerializeField]
    Text unitText;
    [SerializeField]
    Text goldText;
    [SerializeField]
    public int Gold;
    [SerializeField]
    Text Round;
    [SerializeField]
    Text Round2;
    [SerializeField]
    Text Wins;
    [SerializeField]
    Text Wins2;
    [SerializeField]
    AnimationCurve goldJuiceX;
    public string enemyFormation;
    [SerializeField]
    Sprite[] resultSprites;
    [SerializeField]
    float endBattleTime = 5;
    [SerializeField]
    GameObject resultObj;

    [Header("Curves and Things")]
    [SerializeField]
    public AnimationCurve JiggleX;
    [SerializeField]
    public AnimationCurve JiggleY;
    public float jiggleTime;
    public GameObject collisionParticle;
    public AnimationCurve attackCurve;
    public float buffTime = 0.6f;
    public AnimationCurve buffX;
    public AnimationCurve buffY;
    public GameObject Buff;
    public GameObject RangedAttack;
    public ParticleSystem levelUpParticles;
    public ParticleSystem deathParticles;
    public ParticleSystem cloudParticles;
    public ParticleSystem coinParticles;
    public float attackTime;
    public Sprite[] qualitySprites;
    public Sprite[] levelSprites;
    public TextAsset database;
    public AnimationCurve buffJuice;
    [SerializeField]
    AnimationCurve resultJuice;
    [SerializeField]
    List<Button> freezeButtons;
    Transform CameraTrans;
    [HideInInspector]
    public bool loadFailed = false;
    public Color zombieColor;
    public Color textColor;
    int gameSpeed;
    [SerializeField]
    Sprite[] GameSpeedSprites;
    [SerializeField]
    Image gameSpeedButtonSR;
    public AnimationCurve gsjuice;
    public GameObject damageText;

    [Header ("Sounds")]
    [SerializeField]
    AudioSource click;
    [SerializeField]
    AudioSource hit;
    [SerializeField]
    AudioSource win;
    [SerializeField]
    AudioSource lose;
    [SerializeField]
    AudioSource draw;    
    [SerializeField]
    AudioSource denied;
    

    private void Start()
    {
        ResetStats();
        CameraTrans = FindObjectOfType<Camera>().transform;
        if (LOCALTESTING)
        {
            databasetext = database.text;
        }
        else 
        {
            //GetComponent<DataBase>().GetDataBase();
        }
    }

    private void ResetStats()
    {
        livesText.text = lives.ToString();
        livesText2.text = lives.ToString();
        Round.text = (round+1).ToString();
        Round2.text = (round+1).ToString();
        Wins.text = wins.ToString();
        Wins2.text = wins.ToString();
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
                    }
                    else if (square != null && !square.GetComponent<GameSquare>().occupied)
                    {
                        //  MOVE THE SQUARE 
                        draggingObj.transform.position = new Vector3(square.transform.position.x, square.transform.position.y, 0);
                        square.GetComponent<GameSquare>().occupied = true;
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj.GetComponent<Unit>().ShowBuffStuff();
                        draggingObj.GetComponent<ShopSprite>().OnMouseExit();
                        draggingObj.GetComponent<ShopSprite>().StopAllMouseOvers();
                        draggingObj = null;
                        playClick();
                    }
                    else if (square != null && square.GetComponent<GameSquare>().occupier != null && draggingObj.name == square.GetComponent<GameSquare>().occupier.name && square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().level != 3 && draggingObj.GetComponent<Unit>().level != 3)
                    {
                        //COMBINE 
                        GameObject oc = square.GetComponent<GameSquare>().occupier;
                        StartCoroutine(oc.GetComponent<Unit>().Combine(draggingObj.GetComponent<Unit>().level * draggingObj.GetComponent<Unit>().level + draggingObj.GetComponent<Unit>().exp - draggingObj.GetComponent<Unit>().level - 1));
                        oc.GetComponent<Unit>().ShowBuffStuff();
                        Destroy(draggingObj);
                    }
                    else if (square != null && square.GetComponent<GameSquare>().occupier != null)
                    {
                        GameObject oc = square.GetComponent<GameSquare>().occupier;
                        draggingObj.transform.position = new Vector3(square.transform.position.x, square.transform.position.y, 0);
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        Collider2D oldsquare = Physics2D.OverlapPoint(draggingObj.GetComponent<ShopSprite>().origin, squares);
                        oc.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        oldsquare.GetComponent<GameSquare>().occupier = oc;
                        oldsquare.GetComponent<GameSquare>().occupied = true;
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        StartCoroutine(oc.GetComponent<Unit>().Jiggle());
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        draggingObj.GetComponent<Unit>().ShowBuffStuff();
                        draggingObj.GetComponent<ShopSprite>().OnMouseExit();
                        draggingObj.GetComponent<ShopSprite>().StopAllMouseOvers();
                        draggingObj = null;
                        playClick();
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
                        playClick();
                    }
                }
                else
                {
                    if (Gold < 3)
                    {
                        StartCoroutine(GoldJuice());
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        denied.Play();
                        draggingObj = null;
                    }
                    else if (square != null && square.name == "Sell")
                    {
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                        playClick();
                    }
                    else if (square != null && !square.GetComponent<GameSquare>().occupied && unitNumber < 6)
                    {
                        //Buy onto new Square
                        draggingObj.transform.position = new Vector3(square.transform.position.x, square.transform.position.y, 0);
                        square.GetComponent<GameSquare>().occupied = true;
                        square.GetComponent<GameSquare>().occupier = draggingObj;
                        draggingObj.GetComponent<ShopSprite>().Bought();
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        draggingObj.transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        draggingObj.transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        draggingObj.GetComponent<Unit>().spriteQuality.SetActive(false);
                        draggingObj.transform.GetChild(0).GetChild(3).position = new Vector2(draggingObj.transform.position.x, draggingObj.transform.position.y - 1.5f);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj.GetComponent<Unit>().unitSound.Play();
                        draggingObj.GetComponent<Unit>().ShowBuffStuff();
                        draggingObj.GetComponent<ShopSprite>().OnMouseExit();
                        draggingObj.GetComponent<ShopSprite>().StopAllMouseOvers();
                        draggingObj = null;
                        Gold -= 3;
                        FindObjectOfType<Shop>().CheckForOwned();
                    }
                    else if (square != null && square.GetComponent<GameSquare>().occupier != null && draggingObj.name == square.GetComponent<GameSquare>().occupier.name && square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().level != 3)
                    {
                        //combine 
                        GameObject oc = square.GetComponent<GameSquare>().occupier;
                        StartCoroutine(oc.GetComponent<Unit>().Combine());
                        oc.GetComponent<ShopSprite>().Bought();
                        oc.GetComponent<Unit>().ShowBuffStuff();
                        Destroy(draggingObj);
                        Gold -= 3;
                    }
                    else
                    {
                        draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                        draggingObj.transform.GetChild(0).gameObject.SetActive(true);
                        StartCoroutine(draggingObj.GetComponent<Unit>().Jiggle());
                        draggingObj = null;
                        playClick();
                        if (unitNumber > 5)
                        {
                            StartCoroutine(UnitJuice());
                            denied.Play();
                        }
                    }
                }
            }
        }
        unitNumber = transform.childCount - 3;
        unitText.text = unitNumber.ToString() + "/6";
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

    public IEnumerator UnitJuice()
    {
        float timer = 0;
        Vector2 u = unitText.transform.position;
        while (timer < 0.6)
        {
            unitText.transform.position = new Vector2(u.x + goldJuiceX.Evaluate(timer), u.y);
            timer += Time.deltaTime;
            yield return null;
        }
        unitText.transform.position = u;
    }

    public void BattleTrigger()
    {
        StartCoroutine(Battle());
        BattleButton.interactable = false;
    }

    public IEnumerator Battle()
    {
        Battling = true;

        GetPlayerUnits();

        foreach (GameObject u in playerUnits)
        {
            u.GetComponent<Unit>().healthPreEndTurn = u.GetComponent<Unit>().health;
            u.GetComponent<Unit>().attackPreEndTurn = u.GetComponent<Unit>().attack;
            u.GetComponent<Unit>().levelPreEndTurn = u.GetComponent<Unit>().level;
            u.GetComponent<Unit>().expPreEndTurn = u.GetComponent<Unit>().exp;
        }


        foreach (GameObject u in allUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnEndTurn());
            while (u.GetComponent<Unit>().actioning)
            {
                yield return null;
            }
        }

        foreach(Button b in freezeButtons)
        {
            b.interactable = false;
        }

        yield return new WaitForSeconds(1);

        foreach (GameObject u in playerUnits)
        {
            if (u.name.Contains("Apprentice")) continue;
            u.GetComponent<Unit>().healthPreBattle = u.GetComponent<Unit>().health;
            u.GetComponent<Unit>().attackPreBattle = u.GetComponent<Unit>().attack;
            u.GetComponent<Unit>().levelPreBattle = u.GetComponent<Unit>().level;
            u.GetComponent<Unit>().expPreBattle = u.GetComponent<Unit>().exp;
        }

        GetPlayerUnits();



        string formation = "[";
        List<Unit> unitsInOrder = new List<Unit>();


        int i = 0;
        for (int y = -2; y < 1; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Collider2D square = Physics2D.OverlapPoint(new Vector2(1.25f * x - 2.5f, y * 1.25f), squares);
                if (square != null && square.GetComponent<GameSquare>().occupied)
                {
                    Unit thisUnit = square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                    formation += thisUnit.symbol;
                    unitsInOrder.Add(thisUnit);
                    //set Stats
                }
                else formation += '.';
                i++;
            }
        }
        formation += ']';
        foreach(Unit u in unitsInOrder)
        {
            formation += '[';
            formation += u.level;
            formation += ',';
            formation += u.attack;
            formation += ',';
            formation += u.health;
            formation += ']';
        }
        formation += '[';
        if (!LOCALTESTING) StartCoroutine(GetComponent<DataBase>().FindOpponent(round.ToString(), formation));
        else
        {
            enemyFormation = testEnemy;
        }

        while (GetComponent<DataBase>().loading)
        {
            yield return null;
        }

        if (loadFailed)
        {
            foreach (Button b in freezeButtons)
            {
                b.interactable = true;
            }
            BattleButton.interactable = true;
            loadFailed = false;
            foreach (GameObject u in playerUnits)
            {
                u.GetComponent<Unit>().health = u.GetComponent<Unit>().healthPreEndTurn;
                u.GetComponent<Unit>().attack = u.GetComponent<Unit>().attackPreEndTurn;
                u.GetComponent<Unit>().level = u.GetComponent<Unit>().levelPreEndTurn;
                u.GetComponent<Unit>().exp = u.GetComponent<Unit>().expPreEndTurn;
            }
            yield break;
        }


        //if(enemyFormation == null || enemyFormation == "") enemyFormation = enemyFormation = "[............p.p.p.][1,100,100][1,100,100][1,100,100][";
        string[] sections = enemyFormation.Split('[');

        //Spawn Enemies

        i = 0;
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
                        newUnit.transform.GetChild(0).GetChild(4).gameObject.SetActive(false);
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
        Color invis = Color.white;
        invis.a = 0;

        while(timer < 2)
        {
            CameraTrans.position = new Vector3(CameraTrans.position.x, Mathf.Lerp(-3.87f, 0.5f, timer / 2), -10);
            timer += Time.deltaTime;
            yield return null;
        }

        
        allUnits = InsertionSort(allUnits);
        

        foreach (GameObject u in allUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnStartOfBattle());
            //if (u.GetComponent<Unit>().actioning) yield return new WaitForSeconds(buffTime);
            yield return new WaitForEndOfFrame();
            while (IsAUnitActioning()) yield return null;
            while (IsAUnitActioning()) yield return null;

        }

        yield return new WaitForSeconds(1);

        foreach (GameObject u in allUnits)
        {
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
            if (frontMostPlayerUnit != null && frontMostEnemyUnit != null)
            {
                StartCoroutine(frontMostPlayerUnit.OnAttack());
                StartCoroutine(frontMostEnemyUnit.OnAttack());
                while (frontMostPlayerUnit.actioning || frontMostEnemyUnit.actioning)
                {
                    yield return null;
                }
                if (frontMostEnemyUnit.health > 0 && frontMostPlayerUnit.health > 0)
                {
                    StartCoroutine(frontMostPlayerUnit.Attack());
                    StartCoroutine(frontMostEnemyUnit.Attack());
                }
                while (frontMostPlayerUnit.attacking || frontMostEnemyUnit.attacking)
                {
                    yield return null;
                }
            }
            //check if any unit is doing an action
            while (IsAUnitActioning()) yield return null;
            while (IsAUnitActioning()) yield return null;

            yield return new WaitForSeconds(buffTime / 2);

            GetPlayerUnits();

            if (playerUnits.Count == 0 && enemyUnits.Count == 00)
            {
                resultObj.GetComponent<Image>().sprite = resultSprites[2];
                Battling = false;
                draw.PlayDelayed(2);
                StartCoroutine(ResultJuice("d"));

            }
            else if (enemyUnits.Count == 0 && playerUnits.Count > 0)
            {
                Battling = false;
                wins++;
                resultObj.GetComponent<Image>().sprite = resultSprites[0];
                if (wins == 10) resultObj.GetComponent<Image>().sprite = resultSprites[4];
                win.PlayDelayed(2);
                StartCoroutine(ResultJuice("w"));
            }
            else if (playerUnits.Count == 0) { 
                Battling = false;
                lives -= 1;
                resultObj.GetComponent<Image>().sprite = resultSprites[1];
                if (lives <= 0) resultObj.GetComponent<Image>().sprite = resultSprites[3];
                lose.PlayDelayed(2);
                StartCoroutine(ResultJuice("l"));
            }

            yield return null;
        }

       


        yield return new WaitForEndOfFrame();
        StartCoroutine(FindObjectOfType<UIClouds>().Enter());
        yield return new WaitForSeconds(1.5f);
        timer = 0;
        
        Color textColorInvis = textColor;
        textColorInvis.a = 0;
        

        while (timer < endBattleTime)
        {
            CameraTrans.position = new Vector3(CameraTrans.position.x, Mathf.Lerp(0.5f, -3.87f, timer / endBattleTime), -10);
            resultObj.GetComponent<Image>().color = Color.Lerp(invis, Color.white, resultJuice.Evaluate(timer / endBattleTime));
            livesText2.color = Color.Lerp(invis, textColor, resultJuice.Evaluate(timer / endBattleTime));
            livesText2.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(invis, Color.white, resultJuice.Evaluate(timer / endBattleTime));
            Wins2.color = Color.Lerp(invis, textColor, resultJuice.Evaluate(timer / endBattleTime));
            Wins2.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(invis, Color.white, resultJuice.Evaluate(timer / endBattleTime));
            Round2.color = Color.Lerp(invis, textColor, resultJuice.Evaluate(timer / endBattleTime));
            Round2.transform.GetChild(0).GetComponent<Image>().color = Color.Lerp(invis, Color.white, resultJuice.Evaluate(timer / endBattleTime));
            timer += Time.deltaTime;
            yield return null;
        }
        resultObj.GetComponent<Image>().color = invis;
        livesText2.transform.GetChild(0).GetComponent<Image>().color = invis;
        Wins2.transform.GetChild(0).GetComponent<Image>().color = invis;
        Round2.transform.GetChild(0).GetComponent<Image>().color = invis;
        Round2.color = textColorInvis;
        Wins2.color = textColorInvis;
        livesText2.color = textColorInvis;
        CameraTrans.position = new Vector3(CameraTrans.position.x, -3.87f,-10);

        if (wins == 10 || lives <= 0) {
            PlayerPrefs.SetString("formation", formation);
            PlayerPrefs.SetInt("wins", wins);
            PlayerPrefs.SetInt("round", round);
            PlayerPrefs.SetInt("lives", lives);
            SceneManager.LoadScene(1);
        }

        foreach (GameObject u in enemyUnits)
        {
            Collider2D square = Physics2D.OverlapPoint(u.transform.position, enemysquares);
            square.GetComponent<GameSquare>().occupied = false;
            square.GetComponent<GameSquare>().occupier = null;
            Destroy(u);
        }

        foreach(GameObject u in playerUnits)
        {
            if (u.GetComponent<Unit>().temperary)
            {
                u.GetComponent<Unit>().health = 0;
            }
        }

        StartCoroutine(FindObjectOfType<UIClouds>().Leave());
        round++;
        ResetStats();
        Gold = 10;

        GetPlayerUnits();
        foreach (GameObject u in playerUnits)
        {
            if (!u.GetComponent<Unit>().dead)
            {
                Instantiate(cloudParticles, u.transform.position, Quaternion.identity);
                yield return new WaitForSeconds(0.1f);
            }
            u.GetComponent<Unit>().health = u.GetComponent<Unit>().healthPreBattle;
            u.GetComponent<Unit>().attack = u.GetComponent<Unit>().attackPreBattle;
            u.GetComponent<Unit>().dead = false;
        }

        yield return new WaitForSeconds(2.3f);
        foreach (GameObject u in playerUnits)
        {
            StartCoroutine(u.GetComponent<Unit>().OnStartOfTurn());
            if (u.GetComponent<Unit>().actioning) yield return new WaitForSeconds(buffTime);
        }

        if (round == 2) StartCoroutine(FindObjectOfType<Shop>().UnlockBuilding(1));  
        else if (round == 4) StartCoroutine(FindObjectOfType<Shop>().UnlockBuilding(2));  
        else if (round == 6) StartCoroutine(FindObjectOfType<Shop>().UnlockBuilding(3));  
        else if (round == 8) StartCoroutine(FindObjectOfType<Shop>().UnlockBuilding(4));

        FindObjectOfType<Shop>().ReRoll(true);
        foreach (Button b in freezeButtons)
        {
            b.interactable = true;
        }
        BattleButton.interactable = true;
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
            if (transform.GetChild(i).GetComponent<Unit>().health <= 0) continue;
            if (transform.GetChild(i).GetComponent<Unit>().playerUnit) playerUnits.Add(transform.GetChild(i).gameObject);
            else enemyUnits.Add(transform.GetChild(i).gameObject);
            allUnits.Add(transform.GetChild(i).gameObject);
        }
    }

    public bool IsAUnitActioning()
    {
        bool result = false;
        for (int i = 3; i < transform.childCount; i++)
        {
            Unit unit = transform.GetChild(i).GetComponent<Unit>();
            if (unit.actioning) result = true;
        }
        return result;
    }

    public void playClick()
    {
        click.pitch = Random.Range(1.3f, 1.7f);
        click.Play();
    }
    public void playHit()
    {
        hit.pitch = Random.Range(0.8f, 1.2f);
        hit.Play();
    }

    public void ToggleGameSpeed()
    {
        gameSpeed ++;
        if (gameSpeed == 3) gameSpeed = 0;
        gameSpeedButtonSR.sprite = GameSpeedSprites[gameSpeed];
        if(gameSpeed == 0)
        {
            jiggleTime = 0.7f;
            attackTime = 1;
            buffTime = 0.6f;
        }
        else if(gameSpeed == 1)
        {
            jiggleTime = 0.525f;
            attackTime = 0.75f;
            buffTime = 0.45f;
        }
        else
        {
            jiggleTime = 0.35f;
            attackTime = 0.5f;
            buffTime = 0.3f;
        }

        GameObject[] us = GameObject.FindGameObjectsWithTag("ShopItem");
        foreach (GameObject u in us)
        {
            u.GetComponent<Unit>().attackTime = attackTime;
            u.GetComponent<Unit>().buffTime = buffTime;
            u.GetComponent<Unit>().jiggleTime = jiggleTime;
        }
    }

    private IEnumerator ResultJuice(string name)
    {
        yield return new WaitForSeconds(4);
        float timer = 0;
        if (name == "l")
        {
            livesText2.text = (lives).ToString();
            StartCoroutine(UIJuice(livesText2.transform));
            if (lives >= 1) Round2.text = (round + 2).ToString();
            StartCoroutine(UIJuice(Round2.transform));
        }
        else if (name == "w")
        {
            Wins2.text = (wins).ToString();
            StartCoroutine(UIJuice(Wins2.transform));
            Round2.text = (round + 2).ToString();
            StartCoroutine(UIJuice(Round2.transform));
        }
        else
        {
            Round2.text = (round + 2).ToString();
            StartCoroutine(UIJuice(Round2.transform));
        }
        timer = 0;
        while (timer < 1)
        {
            timer += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator UIJuice(Transform t)
    {
        float timer = 0;
        while(timer < 1)
        {
            t.localScale = new Vector3(2 * JiggleX.Evaluate(timer), 2 * JiggleY.Evaluate(timer), 1);
            timer += Time.deltaTime;
            yield return null;
        }
        t.localScale = new Vector3(2, 2, 1);
    }
}
