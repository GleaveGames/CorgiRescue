﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManaBar : MonoBehaviour
{
    public int mana;
    Image[] sprites;
    [SerializeField]
    Color emtpyColor;
    [SerializeField]
    Color fullColor;
    [SerializeField]
    float manaspeed;
    private BuildButtons bb;

    // Start is called before the first frame update
    void Start()
    {
        bb = transform.parent.GetComponent<BuildButtons>();
        sprites = new Image[transform.childCount];
        for(int i = 0; i < transform.childCount; i++) 
        {
            sprites[i] = transform.GetChild(i).gameObject.GetComponent<Image>();
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //if mana is full then move it up slightly?
        if(sprites[transform.childCount-1].color != fullColor) 
        {
            for (int i = 0; i <transform.childCount; i++)  
            {
                if (sprites[i].color != fullColor) 
                {
                    sprites[i].color = Color.Lerp(sprites[i].color, fullColor, manaspeed*Time.deltaTime);
                    mana = i+1;
                    bb.CheckAffordability();
                    return;
                }
            }
        }
    }

    public void UseMana(int cost) 
    {
        if(mana != transform.childCount) 
        {
            for (int i = mana; i > mana - 1 - cost && i >= 0; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
        else 
        {
            for (int i = mana-1; i > mana - cost - 1 && i >= 0; i--)
            {
                sprites[i].color = emtpyColor;
            }
        }
    }
}