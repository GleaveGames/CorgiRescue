using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    public int health=1;
    private LevelGenerator lg;
    private AudioManager am;
    [SerializeField]
    private GameObject blood;
    
    private void Start()
    {
        lg = FindObjectOfType<LevelGenerator>();
        am = FindObjectOfType<AudioManager>();
    }


    private void Update()
    {
        if (health < 1)
        {
            lg.livingThings.Remove(gameObject);
            Destroy(gameObject);
            Instantiate(blood, transform.position, Quaternion.identity);
            am.Play("Hit", transform.position, true);
        }            
    }
}
