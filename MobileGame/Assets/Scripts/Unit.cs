using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit : MonoBehaviour
{
    public int health;
    public int attack;
    public int level;
    public int exp;
    Text healthText;
    Text attackText;
    Text levelText;
    Text expText;
    [SerializeField]
    ParticleSystem levelUpParticles;
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
    [SerializeField]
    AnimationCurve attackCurve;
    [SerializeField]
    float attackTime;
    protected GameController gc;
    [HideInInspector]
    public int healthPreBattle;
    [HideInInspector]
    public int attackPreBattle;



    [Header("Buff")]
    [SerializeField]
    protected AnimationCurve buffY;
    [SerializeField]
    protected AnimationCurve buffX;
    [SerializeField]
    protected GameObject Buff;
    [SerializeField]
    protected int attackBuff;
    [SerializeField]
    protected int healthBuff;
    [SerializeField]
    protected float buffTime = 1;


    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        expText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
        gc = FindObjectOfType<GameController>();
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
                square = Physics2D.OverlapPoint(transform.position, playerTiles);
            }
            else
            {
                square = Physics2D.OverlapPoint(transform.position, enemyTiles);
            }
            square.GetComponent<GameSquare>().occupied = false;
            square.GetComponent<GameSquare>().occupier = null;

            if (!gc.Battling)
            {
                if (playerUnit)
                {
                    health = healthPreBattle;
                    attack = attackPreBattle;
                    GetComponent<SpriteRenderer>().enabled = true;
                    transform.GetChild(0).gameObject.SetActive(true);
                    square.GetComponent<GameSquare>().occupied = true;
                    square.GetComponent<GameSquare>().occupier = gameObject;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public virtual IEnumerator OnStartOfBattle()
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
        
        for (int j = 1; j < 4; j++)
        {
            for (int i = 0; i < 6; i++)
            {
                Collider2D square = null;
                Vector2 spawnPoint = Vector2.zero;
                if (playerUnit)
                {
                    spawnPoint = new Vector2(i - 2.5f, j);
                    square = Physics2D.OverlapPoint(spawnPoint, enemyTiles);
                }
                else
                {
                    spawnPoint = new Vector2(i - 2.5f, j-3);
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
                    Vector2 initPos = transform.position;
                    while(timer < attackTime)
                    {
                        transform.position = Vector2.Lerp(initPos, spawnPoint, attackCurve.Evaluate(timer / attackTime));
                        timer += Time.deltaTime;
                        yield return null;
                    }

                        
                    transform.position = initPos;


                    // Code for Damaging a unit
                    Unit enemyUnit = square.GetComponent<GameSquare>().occupier.GetComponent<Unit>();
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
    }

    public virtual void Combine()
    {
        health++;
        attack++;
        exp++;
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

}
