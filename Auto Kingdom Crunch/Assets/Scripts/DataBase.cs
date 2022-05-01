using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
 
public class DataBase : MonoBehaviour
{
    string geturl = "http://gleavegames.xyz/upload.php";
    public bool loading;
    GameController gc;

    public IEnumerator FindOpponent(string round, string formation)
    {
        gc = FindObjectOfType<GameController>();
        loading = true;
        List<IMultipartFormSection> wwwForm = new List<IMultipartFormSection>();
        wwwForm.Add(new MultipartFormDataSection("playerRound", round));
        wwwForm.Add(new MultipartFormDataSection("playerFormation", formation));
        wwwForm.Add(new MultipartFormDataSection("playerName", MainMenu.Instance.username));

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

        using (UnityWebRequest www = UnityWebRequest.Post("http://gleavegames.xyz/login.php", form))
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
                    GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "";
                    yield break;
                }
                jsonArray = jsonArray.Trim('[',']');
                jsonArray = jsonArray.Trim('{','}');
                string[] sections = jsonArray.Split(',');
                int ingameint = int.Parse(sections[4].Split(':')[1].Trim('"'));
                bool ingame = (ingameint == 0 ? false : true);
                //sections[0].Split(':')[1].Trim('"');
                //Debug.Log(sections[3].Split(':')[1].Trim('"'));
                MainMenu.Instance.SetClientInfo(sections[0].Split(':')[1].Trim('"'), sections[1].Split(':')[1].Trim('"'), sections[2].Split(':')[1].Trim('"'), int.Parse(sections[3].Split(':')[1].Trim('"')), ingame, sections[5].Split(':')[1].Trim('"'), int.Parse(sections[6].Split(':')[1].Trim('"')), int.Parse(sections[7].Split(':')[1].Trim('"')), int.Parse(sections[8].Split(':')[1].Trim('"')), sections[9].Split(':')[1].Trim('"'), int.Parse(sections[10].Split(':')[1].Trim('"')));
                FindObjectOfType<Login>().LoginSuccess();
                GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = "";
            }
        }
    }

    public IEnumerator RegisterUser(string username, string password)
    {

        if(username.Length < 4)
        {
            StartCoroutine(FindObjectOfType<Login>().DisplayText("Username must be at least 3 characters"));
            yield break;
        }
        if (username.Contains("[") || username.Contains("]")){
            StartCoroutine(FindObjectOfType<Login>().DisplayText("Usernames must be letters and numbers"));
            yield break;
        }

        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);


        using (UnityWebRequest www = UnityWebRequest.Post("http://gleavegames.xyz/registeruser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                GameObject.FindGameObjectWithTag("LoadingText").GetComponent<Text>().text = www.error;
            }
            else
            {
                StartCoroutine(FindObjectOfType<Login>().DisplayText("Account Created Successfully"));
            }
        }
        StartCoroutine(Login(username, password));
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
        

        using (UnityWebRequest www = UnityWebRequest.Post("http://gleavegames.xyz/updatestats.php", form))
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

    public IEnumerator SetDBShop(string ID, string SHOPFORMATION, int GOLD)
    {
        //string formattedFormation = SAVEDFORMATION.Replace(',', '!');

        WWWForm form = new WWWForm();
        form.AddField("id", ID);
        form.AddField("shopFormation", SHOPFORMATION);
        form.AddField("gold", GOLD);


        using (UnityWebRequest www = UnityWebRequest.Post("http://gleavegames.xyz/updateshop.php", form))
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

