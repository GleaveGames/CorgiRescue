using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
 
public class DataBase : MonoBehaviour
{
    string geturl = "https://feudalwars.000webhostapp.com/upload.php";
    public bool loading;
    public Text loadingText;
    

    public IEnumerator FindOpponent(string round, string formation)
    {
        loading = true;
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("playerRound", round));
        wwwForm.Add(new MultipartFormDataSection("playerFormation", formation));

        UnityWebRequest www = UnityWebRequest.Post(geturl, wwwForm);
        loadingText.text = "loading...";
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            loadingText.text = "Loading Failed, error: " + www.error;
            Debug.Log("errer " + www.error);
        }
        else
        {
            Debug.Log(www.downloadHandler.text);
            GetComponent<GameController>().enemyFormation = www.downloadHandler.text;
            loadingText.text = "";
        }
        loading = false;
    }

    
}

