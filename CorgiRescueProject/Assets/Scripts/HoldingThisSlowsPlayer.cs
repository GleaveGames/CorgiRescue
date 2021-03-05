using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingThisSlowsPlayer : MonoBehaviour
{

    [SerializeField]
    public float percentageSlowed;
    private playerStats ps;
    private float initmovespeed;
    private float slowedSpeed;
    private bool pickedUp = false;

    private void Start()
    {
        ps = FindObjectOfType<playerStats>();
        initmovespeed = ps.moveSpeed;
        slowedSpeed = initmovespeed * (1 - (percentageSlowed / 100));
    }


    // Update is called once per frame
    void Update()
    {
        if (!ps.steeltoes)
        {
            if (!pickedUp)
            {
                if (transform.parent != null)
                {
                    if (transform.root.name.Contains("Player"))
                    {
                        pickedUp = true;
                        ps.moveSpeed = slowedSpeed;
                    }
                }
            }
            else
            {
                if (transform.parent == null)
                {
                    ps.moveSpeed = initmovespeed;
                    pickedUp = false;
                }
            }
        }
    }
}
