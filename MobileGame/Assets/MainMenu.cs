using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static MainMenu Instance;

    public string id;
    public string username;
    public string password;
    public int trophies;
    public bool inGame;
    public string savedFormation;

    public DataBase db;

    private void Start()
    {
        Instance = this;
        db = GetComponent<DataBase>();
    }

    public void SetInfo(string ID, string NAME, int TROPHIES, bool INGAME, string SAVEDFORMATION)
    {
        id = ID;
        username = NAME;
        trophies = TROPHIES;
        inGame = INGAME;
        savedFormation = SAVEDFORMATION;
        Debug.Log("id = " + id);
        Debug.Log("username = " + username);
        Debug.Log("trophies = " + trophies);
        Debug.Log("ingame = " + INGAME);
        Debug.Log("saved formation = " + SAVEDFORMATION);
    }
}
