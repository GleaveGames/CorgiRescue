using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageThisDoes : MonoBehaviour
{
    public int damage;
    [HideInInspector]
    public int initialDamage;

    private void Start()
    {
        initialDamage = damage;
    }

}
