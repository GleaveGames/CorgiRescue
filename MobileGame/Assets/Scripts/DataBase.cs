using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
 
public class DataBase : MonoBehaviour
{
    string geturl = "https://feudalwars.000webhostapp.com/upload.php";
    public bool loading;
    GameController gc;

    public IEnumerator FindOpponent(string round, string formation)
    {
        gc = FindObjectOfType<GameController>();
        loading = true;
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("playerRound", round));
        wwwForm.Add(new MultipartFormDataSection("playerFormation", formation));

        UnityWebRequest www = UnityWebRequest.Post(geturl, wwwForm);
        GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "loading...";
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "Loading Failed, error: " + www.error;
            gc.loadFailed = true;
            Debug.Log("errer " + www.error);
        }
        else
        {
            FindObjectOfType<GameController>().enemyFormation = www.downloadHandler.text;
            GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "";
        }
        loading = false;
    }

    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://feudalwars.000webhostapp.com/login.php", form))
        {
            GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "Loading...";
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = www.error;
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                if (www.downloadHandler.text == "Wrong password." || www.downloadHandler.text == "Username not found." || www.downloadHandler.text[0] != '[')
                {
                    StartCoroutine(FindObjectOfType<Login>().DisplayText(www.downloadHandler.text));
                    yield break;
                }
                jsonArray = jsonArray.Trim('[',']');
                jsonArray = jsonArray.Trim('{','}');
                string[] sections = jsonArray.Split(',');
                int ingameint = int.Parse(sections[4].Split(':')[1].Trim('"'));
                bool ingame = (ingameint == 0 ? false : true);
                //sections[0].Split(':')[1].Trim('"');
                //Debug.Log(sections[3].Split(':')[1].Trim('"'));
                MainMenu.Instance.SetClientInfo(sections[0].Split(':')[1].Trim('"'), sections[1].Split(':')[1].Trim('"'), sections[2].Split(':')[1].Trim('"'), int.Parse(sections[3].Split(':')[1].Trim('"')), ingame, sections[5].Split(':')[1].Trim('"'), int.Parse(sections[6].Split(':')[1].Trim('"')), int.Parse(sections[7].Split(':')[1].Trim('"')), int.Parse(sections[8].Split(':')[1].Trim('"')));
                FindObjectOfType<Login>().LoginSuccess();
                GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "";
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("https://feudalwars.000webhostapp.com/registeruser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }

    public IEnumerator SetDBInfo(string ID, int TROPHIES, bool INGAME, string SAVEDFORMATION, int WINS, int LIVES, int ROUND)
    {
        string formattedFormation = SAVEDFORMATION.Replace(',', '!');

        WWWForm form = new WWWForm();
        form.AddField("id", ID);
        form.AddField("trophies", TROPHIES);
        form.AddField("inGame", INGAME.ToString());
        form.AddField("formation", formattedFormation);
        form.AddField("wins", WINS);
        form.AddField("lives", LIVES);
        form.AddField("round", ROUND);
        

        using (UnityWebRequest www = UnityWebRequest.Post("https://feudalwars.000webhostapp.com/updatestats.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }


}

