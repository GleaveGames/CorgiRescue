using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButtons : MonoBehaviour
{
    [SerializeField]
    List<GameObject> buttons;
    GameManager gm;
    PlayerInput pi;


    // Start is called before the first frame update
    void Start()
    {
        pi = FindObjectOfType<PlayerInput>();
        gm = FindObjectOfType<GameManager>();
        for(int i = 1; i < transform.childCount; i++) 
        {
            buttons.Add(transform.GetChild(i).gameObject);
        }
        
    
    }



}
