﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public string id;
    public string username;
    public string password;
    public int trophies;
    public bool inGame;
    public string savedFormation;
    public int wins;
    public int lives;
    public int round;

    public DataBase db;
    public bool continuing = false;

    private void Start()
    {
        if(MainMenu.Instance != null)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            db = GetComponent<DataBase>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public void SetClientInfo(string ID, string NAME, string PASSWORD, int TROPHIES, bool INGAME, string SAVEDFORMATION, int WINS, int LIVES, int ROUND)
    {
        id = ID;
        username = NAME;
        password = PASSWORD;
        trophies = TROPHIES;
        inGame = INGAME;
        savedFormation = SAVEDFORMATION.Replace('!', ',');
        wins = WINS;
        lives = LIVES;
        round = ROUND;

        Debug.Log("id = " + id);
        Debug.Log("username = " + username);
        Debug.Log("trophies = " + trophies);
        Debug.Log("ingame = " + INGAME);
        Debug.Log("saved formation = " + SAVEDFORMATION);
        FindObjectOfType<MenuInputs>().OnLogin();
    }

    public void SetDBInfo()
    {
        StartCoroutine(db.SetDBInfo(id, trophies, inGame, savedFormation, wins, lives, round));
    }

    public void ResetClientStats()
    {
        id = null;
        username = "";
        password="";
        trophies=0;
        inGame=false;
        savedFormation="";
        wins=0;
        lives=4;
        round=1;
    }

    public void ResetAfterGameEnd()
    {
        inGame = false;
        savedFormation = "";
        wins = 0;
        lives = 4;
        round = 1;
    }
}
