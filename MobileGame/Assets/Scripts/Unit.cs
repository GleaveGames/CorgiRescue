using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit : MonoBehaviour
{
    public char symbol;
    public int quality;
    public int health;
    public int attack;
    public int level;
    public int exp;
    Text healthText;
    Text attackText;
    public Text levelText;
    public Text expText;
    Sprite[] qualitySprites;
    public GameObject spriteQuality;
    ParticleSystem levelUpParticles;
    ParticleSystem deathParticles;    
    ParticleSystem cloudParticles;
    [HideInInspector]
    public bool attacking;
    [HideInInspector]
    public bool actioning;
    [HideInInspector]
    public bool playerUnit = true;
    [SerializeField]
    protected LayerMask playerTiles;
    [SerializeField]
    protected LayerMask enemyTiles;
    AnimationCurve attackCurve;   
    float attackTime;
    protected GameController gc;
    public int healthPreBattle;
    public int attackPreBattle;
    float jiggleTime;
    AnimationCurve jiggleX;
    AnimationCurve jiggleY;
    GameObject collisionParticle;
    Color colorInvisible;

    [Header("Buff")]
    protected AnimationCurve buffY;
    protected AnimationCurve buffX;
    protected GameObject Buff;
    [SerializeField]
    protected int attackBuff;
    [SerializeField]
    protected int healthBuff;
    protected float buffTime = 1;
    [HideInInspector]
    public bool dead;

    [HideInInspector]
    public Vector2 initPos;

    // Start is called before the first frame update
    void Start()
    {
        gc = FindObjectOfType<GameController>();
        qualitySprites = gc.qualitySprites;
        healthText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        expText = transform.GetChild(0).GetChild(4).GetComponent<Text>();
        spriteQuality = transform.GetChild(0).GetChild(6).gameObject;
        spriteQuality.GetComponent<Image>().sprite = qualitySprites[quality - 1];
        jiggleX = gc.JiggleX;
        jiggleY = gc.JiggleY;
        jiggleTime = gc.jiggleTime;
        collisionParticle = gc.collisionParticle;
        colorInvisible = gc.colorInvisible;
        attackCurve = gc.attackCurve;
        buffTime = gc.buffTime;
        Buff = gc.Buff;
        buffX = gc.buffX;
        buffY = gc.buffY;
        levelUpParticles = gc.levelUpParticles;
        deathParticles = gc.deathParticles;
        cloudParticles = gc.cloudParticles;
        Instantiate(cloudParticles, transform.position, Quaternion.identity);
        attackTime = gc.attackTime;
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        levelText.text = level.ToString();
        if(level == 3)
        {
            expText.enabled = false;
        }
        else expText.text = exp.ToString() + "/" + (level + 2).ToString();
        if (level <= exp-2)
        {
            //level up
            LevelUp();
        }
        if(health <= 0)
        {
            if (!dead) StartCoroutine(Die());
        }
    }

    IEnumerator Die()
    {
        dead = true;
        //Unit Dead
        health = 0;
        if (playerUnit) gc.playerUnits.Remove(gameObject);
        else gc.enemyUnits.Remove(gameObject);
        //CANNOT REMOVE FROM ALLUNITS BECAUSE WE ARE ITERATING THROUGH IT
        //gc.allUnits.Remove(gameObject);
        GetComponent<SpriteRenderer>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);
        Collider2D square = null;
        if (playerUnit)
        {
            square = Physics2D.OverlapPoint(initPos, playerTiles);
        }
        else
        {
            square = Physics2D.OverlapPoint(initPos, enemyTiles);
        }
        square.GetComponent<GameSquare>().occupied = false;
        square.GetComponent<GameSquare>().occupier = null;
        Instantiate(deathParticles, square.transform.position, Quaternion.identity);
        while (gc.Battling) yield return null;
        if (playerUnit)
        {
            yield return new WaitForSeconds(Random.Range(0, 0.5f));
            Instantiate(cloudParticles, initPos, Quaternion.identity);
            attack = attackPreBattle;
            health = healthPreBattle;
            yield return new WaitForSeconds(0.1f);
            GetComponent<SpriteRenderer>().enabled = true;
            transform.GetChild(0).gameObject.SetActive(true);
            square.GetComponent<GameSquare>().occupied = true;
            square.GetComponent<GameSquare>().occupier = gameObject;
            transform.position = initPos;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public virtual IEnumerator OnStartOfBattle()
    {
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
        actioning = false;
        yield return null;
    }
    
    public virtual IEnumerator OnDie()
    {
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
        actioning = false;
        yield return null;
    }
    public virtual IEnumerator OnSell()
    {
        actioning = false;
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
                    Debug.Log("Didn't hit a sqaure");
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
                        
                        if (timer > attackTime/2 && !particlesGoneOff)
                        {
                            particlesGoneOff = true;
                            particle = Instantiate(collisionParticle, transform.position, Quaternion.identity);
                            particleSpawnPos = (transform.position + enemyUnit.transform.position)/2;
                            particle.transform.up = Vector2.Perpendicular(spawnPoint - initPos);
                            StartCoroutine(CollisionJiggle());
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
        StartCoroutine(OnLevelUp());
        StartCoroutine(Jiggle());
    }

    public virtual void Combine()
    {
        health++;
        attack++;
        exp++;
        StartCoroutine(Jiggle());
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

}
