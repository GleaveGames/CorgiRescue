using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    private playerStats ps;
    private bool glovesTigger;

    private void Start()
    {
        ps = FindObjectOfType<playerStats>();
    }

    private void Update()
    {
        if (!glovesTigger)
        {
            if (ps.spikegloves)
            {
                glovesTigger = true;
                GetComponent<DamagesPlayer>().canHurtPlayer = false;
            }
        }        
    }
}
