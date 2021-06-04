using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour 
{
    CharacterStats cs;
    Transform fill;

    private void Start()
    {
        cs = transform.parent.gameObject.GetComponent<CharacterStats>();
        fill = transform.GetChild(0);
        fill.GetComponent<SpriteRenderer>().color = FindObjectOfType<GameManager>().teams[cs.team].color;
    }

    private void Update()
    {
        Vector2 scale = new Vector2(1,1);
        scale.x = (float)cs.health / cs.initialHealth;
        Debug.Log(cs.health / cs.initialHealth);
        fill.localScale = scale;
        //////
        Vector2 pos = Vector2.zero;
        pos.x = (1 - ((float)cs.health / cs.initialHealth)) * -0.7357f;
        fill.localPosition = pos;
    }
}
