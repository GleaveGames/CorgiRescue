

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class DataBase : MonoBehaviour
{
    string url = "https://raw.githubusercontent.com/GleaveGames/CorgiRescue/main/MobileGame/Assets/testdatabase.txt";


    public void GetDataBase()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWW w = new WWW(url);
        yield return w;
        Debug.Log(w.text);

        GetComponent<GameController>().databasetext = w.text;
    }
}

