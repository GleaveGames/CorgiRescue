using System.Collections;
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
    public string shopFormation;
    public int gold;

    public DataBase db;
    public bool continuing = false;
    public Texture2D[] mouseCursor;

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

    private void Update()
    {
        if (Input.GetMouseButton(0)) Cursor.SetCursor(mouseCursor[1], Vector2.zero, CursorMode.ForceSoftware);
        else Cursor.SetCursor(mouseCursor[0], Vector2.zero, CursorMode.ForceSoftware);
    }

    public void SetClientInfo(string ID, string NAME, string PASSWORD, int TROPHIES, bool INGAME, string SAVEDFORMATION, int WINS, int LIVES, int ROUND, string SHOPFORMATION, int GOLD)
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
        shopFormation = SHOPFORMATION;
        gold = GOLD;
        var fooGroup = Resources.FindObjectsOfTypeAll<MenuInputs>();
        if (fooGroup.Length > 0)
        {
            var foo = fooGroup[0];
            foo.gameObject.SetActive(true);
        }
        FindObjectOfType<MenuInputs>().OnLogin();
    }


    public void SetShopInfo(string shopFormation, int shopGold)
    {
        StartCoroutine(db.SetDBShop(id, shopFormation, shopGold));
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
        round=0;
    }

    public void ResetAfterGameEnd()
    {
        inGame = false;
        savedFormation = "";
        wins = 0;
        lives = 4;
        round = 0;
    }
}
