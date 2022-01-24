using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Button NewAccountButton;
    [SerializeField]
    TextMeshProUGUI errorText;

    private void Start()
    {
        LoginButton.onClick.AddListener(() => {
            StartCoroutine(MainMenu.Instance.db.Login(UsernameInput.text, PasswordInput.text));
        });
        NewAccountButton.onClick.AddListener(() => {
            StartCoroutine(MainMenu.Instance.db.RegisterUser(UsernameInput.text, PasswordInput.text));
        });
        StartCoroutine(LateStart());
    }

    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.GetString("Username") != null) StartCoroutine(MainMenu.Instance.db.Login(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password")));
    }


    public void LoginSuccess()
    {
        PlayerPrefs.SetString("Username", MainMenu.Instance.username);
        PlayerPrefs.SetString("Password", MainMenu.Instance.password);
        gameObject.SetActive(false);
    }

    public IEnumerator DisplayText(string error)
    {
        errorText.text = error;
        yield return new WaitForSeconds(3);
        errorText.text = "";
    }

}
