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
    public bool attacking;
    public bool playerUnit = true;
    [SerializeField]
    LayerMask playerTiles;
    [SerializeField]
    LayerMask enemyTiles;
    [SerializeField]
    AnimationCurve attackCurve;
    [SerializeField]
    float attackTime;
    GameController gc;

    public int healthPreBattle;
    public int attackPreBattle;

    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
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
        expText.text = exp.ToString() + "/" + level.ToString();
        if(level < exp+1)
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
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    public virtual IEnumerator Attack()
    {
        attacking = true;

        //get nearest enemy 
        if (playerUnit)
        {
            for (int j = 1; j < 4; j++)
            {
                for (int i = 0; i < 6; i++)
                {
                    Vector2 spawnPoint = new Vector2(i-2.5f, j);
                    Collider2D square = Physics2D.OverlapPoint(spawnPoint, enemyTiles);
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

                        square.GetComponent<GameSquare>().occupier.GetComponent<Unit>().health -= attack;

                        goto end;
                    }
                }
            }
        }

        end:
            yield return new WaitForSeconds(1);
            attacking = false;

    }

    protected virtual void LevelUp()
    {
        exp = 0;
        level++;
        Instantiate(levelUpParticles, transform.position, Quaternion.identity);
    }

    public virtual void Combine()
    {
        health++;
        attack++;
        exp++;
    }


}
