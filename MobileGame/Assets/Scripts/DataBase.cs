

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class DataBase : MonoBehaviour
{
    //string url = "https://raw.githubusercontent.com/GleaveGames/CorgiRescue/main/MobileGame/Assets/testdatabase.txt";
    string url = "https://localhost:44369/DataBase";
    string geturl = "https://feudalwars.000webhostapp.com/phpTest.php";


    public void GetDataBase()
    {
        StartCoroutine(GetText());
    } 
    
    /*public void PostDataBase(int round, string formation)
    {
        StartCoroutine(PostText());
    }
    */


    IEnumerator GetText()
    {
        UnityWebRequest w = UnityWebRequest.Get(geturl);
        yield return w.SendWebRequest();
        if (w.isNetworkError || w.isHttpError) Debug.Log("errer " + w.error);
        else
        {
            Debug.Log(w.downloadHandler.text.Length);
            GetComponent<GameController>().databasetext = w.downloadHandler.text;
        }
    }


    IEnumerator FindOpponent(string formation)
    {
        UnityWebRequest w = UnityWebRequest.Get(geturl);
        yield return w.SendWebRequest();
        if (w.isNetworkError || w.isHttpError) Debug.Log("errer " + w.error);
        else
        {
            Debug.Log(w.downloadHandler.text.Length);
            GetComponent<GameController>().databasetext = w.downloadHandler.text;
        }
    }

    
}

