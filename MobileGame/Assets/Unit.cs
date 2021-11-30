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

    



    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
        levelText = transform.GetChild(0).GetChild(2).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
        levelText.text = level.ToString();
        if(level <= exp+1)
        {
            //level up
        }

    }

    protected virtual void Attack()
    {

    }

    protected virtual void LevelUp()
    {
        exp = 0;
        level++;
    }

    public virtual void Combine()
    {
        health++;
        attack++;
        exp++;
    }


}
