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


    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
        expText = transform.GetChild(0).GetChild(3).GetComponent<Text>();
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
    }

    public virtual IEnumerator Attack()
    {
        attacking = true;

        Instantiate(levelUpParticles, transform.position, Quaternion.identity);
        //get nearest enemy 
        if (playerUnit)
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    Vector2 spawnPoint = new Vector2(i, j);
                    Collider2D square = Physics2D.OverlapPoint(spawnPoint, enemysquares);
                }
            }
        }
        
        





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
