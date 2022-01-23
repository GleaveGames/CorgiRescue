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
    GameController gc;

    private void Start()
    {
        //StartCoroutine(Login("king", "king"));
        //gc = FindObjectOfType<GameController>();
        //StartCoroutine(RegisterUser("queen", "1234"));
    }

    public IEnumerator FindOpponent(string round, string formation)
    {
        gc = FindObjectOfType<GameController>();
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
            gc.loadFailed = true;
            Debug.Log("errer " + www.error);
        }
        else
        {
            GetComponent<GameController>().enemyFormation = www.downloadHandler.text;
            loadingText.text = "";
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
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                string jsonArray = www.downloadHandler.text;
                jsonArray = jsonArray.Trim('[',']');
                jsonArray = jsonArray.Trim('{','}');
                string[] sections = jsonArray.Split(',');
                int ingameint = int.Parse(sections[3].Split(':')[1].Trim('"'));
                bool ingame = (ingameint == 0 ? false : true);
                //sections[0].Split(':')[1].Trim('"');
                //Debug.Log(sections[3].Split(':')[1].Trim('"'));
                MainMenu.Instance.SetInfo(sections[0].Split(':')[1].Trim('"'), sections[1].Split(':')[1].Trim('"'), int.Parse(sections[2].Split(':')[1].Trim('"')), ingame, sections[4].Split(':')[1].Trim('"'));
                FindObjectOfType<Login>().LoginSuccess();
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


}

