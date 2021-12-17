

using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
 
public class DataBase : MonoBehaviour
{
    string url = "https://raw.githubusercontent.com/GleaveGames/CorgiRescue/dc04279d9a66e358042d4d1a8b0743d074c51607/MobileGame/testdatabase.txt";

    /*
   private var url = "http://www.textfiles.com/100/adventur.txt";
 
    function Start () {
        var w = new WWW(url);
        print("Loading");
        yield w;
        print(w.url);
        Debug.Log(w.text);
    }

    */

    void Start()
    {
        StartCoroutine(GetText());
    }

    IEnumerator GetText()
    {
        WWW w = new WWW(url);
        yield return w;
        Debug.Log(w.text);
        Debug.Log(w.text.Length);

        GetComponent<GameController>().databasetext = w.text;

        /*

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);

            // Or retrieve results as binary data
            byte[] results = www.downloadHandler.data;
        }
        */
    }
    
}













/*using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DataBase : MonoBehaviour
{
    const string privateCode = "DcR7eEqWmkixNBWVXp9tAgxSxrai0QgkO2duT8YhyEfg";
    const string publicCode = "5f6a393ceb371809c43fbb2a";
    const string webURL = "https://www.dreamlo.com/lb/";

    public Highscore[] highscoreslist;

    private void Awake()
    {
        DownloadHighscores();
    }

    public void AddNewHighscore(string username, int score)
    {
        StartCoroutine(UploadNewHighscore(username, score));
    }


    IEnumerator UploadNewHighscore(string username, int score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(username) + "/" + score);
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            print("upload Successful");
        }
        else
        {
            print("error uploading" + www.error);
        }
    }

    public void DownloadHighscores()
    {
        StartCoroutine("DownloadHighscoresFromDatabase");
    }

    IEnumerator DownloadHighscoresFromDatabase()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighscores(www.text);
        }
        else
        {
            print("error downloading" + www.error);
        }
    }

    void FormatHighscores(string textStream)
    {
        string[] entries = textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        highscoreslist = new Highscore[entries.Length];
        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryInfo = entries[i].Split(new char[] { '|' });
            string username = entryInfo[0];
            int score = int.Parse(entryInfo[1]);
            highscoreslist[i] = new Highscore(username, score);
            print(highscoreslist[i].username + highscoreslist[i].score);
        }
    }
}

public struct Highscore
{
    public string username;
    public int score;

    public Highscore(string _username, int _score)
    {
        username = _username;
        score = _score;
    }
}
*/