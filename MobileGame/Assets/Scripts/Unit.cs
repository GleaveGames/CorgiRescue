using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Unit : MonoBehaviour
{
    [HideInInspector]
    public bool temperary = false;
    public char symbol;
    public int quality;
    public int health;
    public int attack;
    public int level;
    public int exp;
    TextMeshProUGUI healthText;
    TextMeshProUGUI attackText;
    Sprite[] qualitySprites;
    Sprite[] levelSprites;
    Image levelSprite;
    [HideInInspector]
    public GameObject spriteQuality;
    List<Image> spriteQualities;
    ParticleSystem levelUpParticles;
    [HideInInspector]
    protected ParticleSystem deathParticles;
    [HideInInspector]
    protected ParticleSystem coinParticles;    
    protected ParticleSystem cloudParticles;
    public bool attacking;
    public bool actioning;
    [HideInInspector]
    public bool playerUnit = true;
    [SerializeField]
    protected LayerMask playerTiles;
    [SerializeField]
    protected LayerMask enemyTiles;
    protected AnimationCurve attackCurve;   
    [HideInInspector]
    public float attackTime;
    protected GameController gc;
    [HideInInspector]
    public int healthPreBattle;
    [HideInInspector]
    public int attackPreBattle;
    [HideInInspector]
    public int levelPreBattle;
    [HideInInspector]
    public int expPreBattle;
    [HideInInspector]
    public int healthPreEndTurn;
    [HideInInspector]
    public int attackPreEndTurn;
    [HideInInspector]
    public int levelPreEndTurn;
    [HideInInspector]
    public int expPreEndTurn;
    [HideInInspector]
    public float jiggleTime;
    AnimationCurve jiggleX;
    AnimationCurve jiggleY;
    protected GameObject collisionParticle;
    protected Color colorInvisible;
    AnimationCurve buffJuice;
    Color zombieColor;
    GameObject buffStuff;

    [Header("Buff")]
    protected AnimationCurve buffY;
    protected AnimationCurve buffX;
    protected GameObject Buff;
    protected GameObject RangedAttack;
    [SerializeField]
    protected int attackBuff;
    [SerializeField]
    protected int healthBuff;

    [Header("Row?")]
    [SerializeField]
    bool[] rowbuff = new bool[3];     
    [SerializeField]
    bool[] rowdebuff = new bool[3];
    [SerializeField]
    bool[] buffs = new bool[9];
    [SerializeField]
    bool[] debuffs = new bool[9];
    [SerializeField]
    bool[] nounits = new bool[9];


    [HideInInspector]
    public float buffTime = 1;
    [HideInInspector]
    public bool dead;

    [HideInInspector]
    public Vector2 initPos;
    [HideInInspector]
    public AudioSource unitSound;
    protected SoundManager sm;

    // Start is called before the first frame update
    public virtual void Start()
    {
        gc = FindObjectOfType<GameController>();
        sm = FindObjectOfType<SoundManager>();
        qualitySprites = gc.qualitySprites;
        healthText = transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        attackText = transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        levelSprite = transform.GetChild(0).GetChild(2).GetComponent<Image>();
        spriteQuality = transform.GetChild(0).GetChild(4).gameObject;
        spriteQualities = new List<Image>();
        spriteQualities.Add(transform.GetChild(0).GetChild(3).GetChild(2).GetComponent<Image>());
        spriteQualities.Add(transform.GetChild(0).GetChild(3).GetChild(3).GetComponent<Image>());
        spriteQuality.GetComponent<Image>().sprite = qualitySprites[quality - 1];
        foreach(Image i in spriteQualities) i.sprite = qualitySprites[quality - 1];
        levelSprites = gc.levelSprites;
        jiggleX = gc.JiggleX;
        jiggleY = gc.JiggleY;
        jiggleTime = gc.jiggleTime;
        collisionParticle = gc.collisionParticle;
        attackCurve = gc.attackCurve;
        buffTime = gc.buffTime;
        Buff = gc.Buff;
        RangedAttack = gc.RangedAttack;
        buffX = gc.buffX;
        buffY = gc.buffY;
        levelUpParticles = gc.levelUpParticles;
        deathParticles = gc.deathParticles;
        cloudParticles = gc.cloudParticles;
        coinParticles = gc.coinParticles;
        ParticleSystem cp = Instantiate(cloudParticles, transform.position, Quaternion.identity);
        if (transform.parent.name.Contains("Shop")) cp.GetComponent<AudioSource>().Stop();
        attackTime = gc.attackTime;
        buffTime = gc.buffTime;
        jiggleTime = gc.jiggleTime;
        initPos = transform.position;
        buffJuice = gc.buffJuice;
        zombieColor = gc.zombieColor;
        unitSound = GetComponent<AudioSource>();
        if (temperary) spriteQuality.SetActive(false);
    }

    // Update is called once per frame
    public virtual void Update()
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        levelSprite.sprite = levelSprites[level*level + exp - level-1];
        
        if(health <= 0)
        {
            health = 0;
            if (!dead) StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        while (actioning)
        {
            if (health > 0) yield break;
            yield return null;
        }
        if (health > 0) yield break;
        if (dead) yield break; 
        dead = true;
        //Unit Dead
        if (playerUnit) gc.playerUnits.Remove(gameObject);
        else gc.enemyUnits.Remove(gameObject);
        //CANNOT REMOVE FROM ALLUNITS BECAUSE WE ARE ITERATING THROUGH IT
        //gc.allUnits.Remove(gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Collider2D square = null;
        if (playerUnit) square = Physics2D.OverlapPoint(initPos, playerTiles);
        else square = Physics2D.OverlapPoint(initPos, enemyTiles);
        //new for prisoner next if
        if (square.GetComponent<GameSquare>().occupier == gameObject)
        {
            square.GetComponent<GameSquare>().occupied = false;
            square.GetComponent<GameSquare>().occupier = null;
        }
        Instantiate(deathParticles, square.transform.position, Quaternion.identity);
        while (gc.Battling) yield return null;
        actioning = false;
        if (playerUnit && !temperary)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.5f));
            Instantiate(cloudParticles, initPos, Quaternion.identity);
            attack = attackPreBattle;
            health = healthPreBattle;
            level = levelPreBattle;
            exp = expPreBattle;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            square.GetComponent<GameSquare>().occupied = true;
            square.GetComponent<GameSquare>().occupier = gameObject;
            transform.position = initPos;
            GetComponent<SpriteRenderer>().color = Color.white;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator OnStartOfBattle()
    {
        if (temperary || GetComponent<SpriteRenderer>().color == zombieColor)
        {
            actioning = true;
            //check for shephard
            for(int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    if (y == 0 && x == 0) continue;
                    if(CheckForUnit(x, y, "Shephard"))
                    {
                        yield return new WaitForSeconds(buffTime / 3);
                        Vector2 pos = transform.position + new Vector3(x * 1.25f, y * 1.25f,0);
                        Collider2D col = null;
                        if (playerUnit) col = Physics2D.OverlapPoint(pos, playerTiles);
                        else col = Physics2D.OverlapPoint(pos, enemyTiles);
                        GameSquare square = col.GetComponent<GameSquare>();
                        GameObject shep = square.occupier;
                        StartCoroutine(shep.GetComponent<Shephard>().ShephardBuff(gameObject));
                        yield return new WaitForSeconds(buffTime);
                    }
                    if (CheckForUnit(x, y, "Noble"))
                    {
                        yield return new WaitForSeconds(buffTime / 3);
                        Vector2 pos = transform.position + new Vector3(x * 1.25f, y * 1.25f, 0);
                        Collider2D col = null;
                        if (playerUnit) col = Physics2D.OverlapPoint(pos, playerTiles);
                        else col = Physics2D.OverlapPoint(pos, enemyTiles);
                        GameSquare square = col.GetComponent<GameSquare>();
                        GameObject shep = square.occupier;
                        StartCoroutine(shep.GetComponent<Shephard>().ShephardBuff(gameObject));
                        yield return new WaitForSeconds(buffTime);
                    }
                }
            }
        }
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnAttack()
    {
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnStartOfTurn()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        actioning = false;
        yield return null;
    }
    
    public virtual IEnumerator OnDie()
    {
        actioning = true;
        bool respawn = false;
        //PRIEST STUFF
    
        GameSquare square = null;

        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, -1.25f), playerTiles);
            if (squareCol != null) square = squareCol.GetComponent<GameSquare>();
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3(0, 1.25f), enemyTiles);
            if (squareCol != null) square = squareCol.GetComponent<GameSquare>();
        }
        if (square != null && square.occupied && square.occupier.name.Contains("Priest") && !square.occupier.GetComponent<Priest>().triggered && square.occupier.GetComponent<Priest>().health > 0) respawn = true;
        if (respawn)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Instantiate(cloudParticles, transform.position, Quaternion.identity);
            dead = false;
            square.occupier.GetComponent<Priest>().triggered = true;
            health = 1;
            attack = 1;
            level = square.occupier.GetComponent<Priest>().level;
            exp = 1;
            GetComponent<SpriteRenderer>().color = zombieColor;
            StartCoroutine(OnStartOfBattle());
            while (actioning) yield return null;
        }

        actioning = false;
        yield return null;
    } 
    
    public virtual IEnumerator OnBuy()
    {
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnHurt()
    {
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnLevelUp()
    {
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnEndTurn()
    {
        initPos = transform.position;
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnSell()
    {
        actioning = false;
        Instantiate(coinParticles, transform.position, Quaternion.identity);
        Destroy(gameObject);
        yield return null;
    }
    public virtual IEnumerator Attack()
    {
        attacking = true;

        //get nearest enemy 
        
        for (int y = 1; y < 4; y++)
        {
            for (int x = 0; x < 6; x++)
            {
                Collider2D square = null;
                Vector2 spawnPoint = Vector2.zero;
                if (playerUnit)
                {
                    spawnPoint = new Vector2(x*1.25f - 2.5f, y*1.25f);
                    square = Physics2D.OverlapPoint(spawnPoint, enemyTiles);
                }
                else
                {
                    spawnPoint = new Vector2(x*1.25f - 2.5f, 1.25f-y*1.25f);
                    square = Physics2D.OverlapPoint(spawnPoint, playerTiles);
                }
                if (square == null) {
                    Instantiate(levelUpParticles, spawnPoint, Quaternion.identity);
                }
                else if (square.GetComponent<GameSquare>().occupied)
                {
                    //Enemy here and attack

                    float timer = 0;
                    initPos = transform.position;
                    Unit enemyUnit = square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
                    bool particlesGoneOff = false;
                    GameObject particle = null;
                    Vector2 particleSpawnPos = new Vector2(0,0);
                    Vector3 initParticleScale = new Vector3(0.2f,0.2f,1);
                    while (timer < attackTime)
                    {

                        //Ping Off

                        if (timer > attackTime / 2 && !particlesGoneOff)
                        {
                            particlesGoneOff = true;
                            particle = Instantiate(collisionParticle, transform.position, Quaternion.identity);
                            particleSpawnPos = (transform.position + enemyUnit.transform.position) / 2;
                            particle.transform.up = Vector2.Perpendicular(spawnPoint - initPos);
                            StartCoroutine(CollisionJiggle());
                            if(playerUnit) gc.playHit();
                        }
                        else if(timer > attackTime / 2)
                        {
                            particle.transform.position = Vector2.Lerp(particleSpawnPos, particleSpawnPos + Vector2.Perpendicular(spawnPoint - initPos)/5, (timer-attackTime/2)/(attackTime/2));
                            particle.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, colorInvisible, (timer - attackTime / 2) / (attackTime / 2));
                            particle.transform.localScale = Vector3.Lerp(initParticleScale, new Vector3(1,1,1), (timer - attackTime / 2) / (attackTime / 2));
                        }

                            /*
                        if (timer > attackTime/2 && enemyUnit.attack >= health)
                            transform.position = Vector2.Lerp((initPos-spawnPoint)*5, spawnPoint, dieCurve.Evaluate(timer / attackTime));
                        else 
                            transform.position = Vector2.Lerp(initPos, spawnPoint, attackCurve.Evaluate(timer / attackTime));
                        */
                        transform.position = Vector2.Lerp(initPos, spawnPoint, attackCurve.Evaluate(timer / attackTime));
                        timer += Time.deltaTime;
                        yield return null;
                    }
                    Destroy(particle);

                    // Code for Damaging a unit
                    enemyUnit.health -= attack;
                    if(enemyUnit.health > 0)
                    {
                        StartCoroutine(enemyUnit.OnHurt());
                        while (enemyUnit.actioning)
                        {
                            yield return null;
                        }
                    }
                    else
                    {
                        StartCoroutine(enemyUnit.OnDie());
                        while (enemyUnit.actioning)
                        {
                            yield return null;
                        }
                    }
                    goto end;
                }
            }
        }

        end:
            yield return new WaitForSeconds(1);
            attacking = false;
    }

    protected virtual void LevelUp()
    {
        exp = 1;
        level++;
        Instantiate(levelUpParticles, transform.position, Quaternion.identity);
        StartCoroutine(FindObjectOfType<Shop>().SpawnNewShopSpot());
        StartCoroutine(OnLevelUp());
        StartCoroutine(Jiggle());
    }

    public virtual IEnumerator Combine(int lxp = 0)
    {
        for (int i = 0; i < lxp + 1; i++)
        {
            health++;
            attack++;
            exp++;
            StartCoroutine(Jiggle());
            if (level <= exp - 2)
            {
                LevelUp();
                sm.PlayLevelUp();
            }
            else sm.PlayExp();
            yield return new WaitForSeconds(0.4f);
        }
    }

    public List<GameObject> GetAllies()
    {
        List<GameObject> allies = new List<GameObject>(); 

        if (playerUnit)
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (unit.playerUnit && unit.health > 0) allies.Add(gc.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (!unit.playerUnit && unit.health>0) allies.Add(gc.transform.GetChild(i).gameObject);
            }
        }

        return allies;
    }

    public List<GameObject> GetEnemies()
    {
        List<GameObject> enemies = new List<GameObject>();

        if (!playerUnit)
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (unit.playerUnit && unit.health > 0) enemies.Add(gc.transform.GetChild(i).gameObject);
            }
        }
        else
        {
            for (int i = 3; i < gc.transform.childCount; i++)
            {
                Unit unit = gc.transform.GetChild(i).GetComponent<Unit>();
                if (!unit.playerUnit && unit.health > 0) enemies.Add(gc.transform.GetChild(i).gameObject);
            }
        }

        return enemies;
    }

    public IEnumerator Jiggle()
    {
        float timer = 0;
        while(timer <= jiggleTime)
        {
            transform.localScale = new Vector3(jiggleX.Evaluate(timer / jiggleTime), jiggleY.Evaluate(timer / jiggleTime),0);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(1, 1, 1);
    }
    public IEnumerator CollisionJiggle()
    {
        float timer = 0;
        while(timer <= jiggleTime/2)
        {
            transform.localScale = new Vector3(jiggleX.Evaluate(2*timer / jiggleTime), jiggleY.Evaluate(2*timer / jiggleTime),0);
            timer += Time.deltaTime;
            yield return null;
        }
        transform.localScale = new Vector3(1, 1, 1);
    }

    public IEnumerator BuffJuice(int buff) 
    {
        float timer = 0;
        float juiceTime = 0.5f;
        while (timer < juiceTime)
        {
            float newScale = buffJuice.Evaluate(timer / juiceTime);
            if(buff == 1) healthText.transform.parent.localScale = new Vector2(newScale, newScale);
            else if(buff == 2) attackText.transform.parent.localScale = new Vector2(newScale, newScale);
            else
            {
                healthText.transform.parent.localScale = new Vector2(newScale, newScale);
                attackText.transform.parent.localScale = new Vector2(newScale, newScale);
            }
            timer += Time.deltaTime;
            yield return null;
        }
        healthText.transform.parent.localScale = new Vector2(0.5f, 0.5f);
    }

    protected bool CheckForUnit(int x, int y, string name, Transform t = null)
    {
        if (t == null) t = transform;
        GameSquare square = null;
        if (playerUnit)
        {
            Collider2D squareCol = Physics2D.OverlapPoint(t.position + new Vector3(x * 1.25f, y * 1.25f), playerTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        else
        {
            Collider2D squareCol = Physics2D.OverlapPoint(t.position + new Vector3(x * 1.25f, y * 1.25f), enemyTiles);
            if (squareCol != null)
            {
                square = squareCol.GetComponent<GameSquare>();
            }
        }
        if (square != null && square.occupied && square.occupier.name.Contains(name)) return true;
        else return false;
    }

    public void ShowBuffStuff()
    {
        Transform board = gc.gameObject.transform.GetChild(0);
        if (rowbuff[0])
        {
            for(int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("D")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(0));
            }
        }
        if (rowbuff[1])
        {
            for (int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("E")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(0));
            }
        }
        if (rowbuff[2])
        {
            for (int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("F")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(0));
            }
        }
        if (rowdebuff[0])
        {
            for (int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("D")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(1));
            }
        }
        if (rowdebuff[1])
        {
            for (int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("E")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(2));
            }
        }
        if (rowdebuff[2])
        {
            for (int i = 0; i < board.childCount; i++)
            {
                if (board.GetChild(i).name.Contains("F")) StartCoroutine(board.GetChild(i).GetComponent<GameSquare>().showBuff(3));
            }
        }

        for(int i = 0; i < buffs.Length; i++)
        {
            if (buffs[i])
            {
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3((i % 3) * 1.25f - 1.25f, 1.25f - (i / 3) * 1.25f), playerTiles);
                if(squareCol != null) StartCoroutine(squareCol.GetComponent<GameSquare>().showBuff(0));
            }
        }
        for (int i = 0; i < debuffs.Length; i++)
        {
            if (debuffs[i])
            {
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3((i % 3) * 1.25f - 1.25f, 1.25f - (i / 3) * 1.25f), playerTiles);
                if (squareCol != null) StartCoroutine(squareCol.GetComponent<GameSquare>().showBuff(1));
            }
        }
        for (int i = 0; i < nounits.Length; i++)
        {
            if (nounits[i])
            {
                Collider2D squareCol = Physics2D.OverlapPoint(transform.position + new Vector3((i % 3) * 1.25f - 1.25f, 1.25f - (i / 3) * 1.25f), playerTiles);
                if (squareCol != null && squareCol.GetComponent<GameSquare>().occupier != null) StartCoroutine(squareCol.GetComponent<GameSquare>().showBuff(1));
                else if(squareCol!=null) StartCoroutine(squareCol.GetComponent<GameSquare>().showBuff(0));
            }
        }
    }
}
