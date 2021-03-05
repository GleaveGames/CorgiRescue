using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressurePlate : MonoBehaviour
{

    //General Triggering Script
    //Use other script to check if this has been triggered with the bool

    public bool triggered = false;
    [SerializeField]
    private Sprite down;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.name.Contains("Bat"))
        {
            triggered = true;
            GetComponent<SpriteRenderer>().sprite = down;
            GetComponent<Animator>().Play("PressurePlateDown");
        }
    }
}
