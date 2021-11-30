using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Unit : MonoBehaviour
{
    public int health;
    public int attack;
    Text healthText;
    Text attackText;



    // Start is called before the first frame update
    void Start()
    {
        healthText = transform.GetChild(0).GetChild(0).GetComponent<Text>();
        attackText = transform.GetChild(0).GetChild(1).GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = health.ToString();
        attackText.text = attack.ToString();
    }

    protected virtual void Attack()
    {

    }
}
