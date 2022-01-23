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

    private void Start()
    {
        LoginButton.onClick.AddListener(() => {
            StartCoroutine(MainMenu.Instance.db.Login(UsernameInput.text, PasswordInput.text));
        });
        NewAccountButton.onClick.AddListener(() => {
            StartCoroutine(MainMenu.Instance.db.RegisterUser(UsernameInput.text, PasswordInput.text));
        });
    }

    public void LoginSuccess()
    {
        Destroy(gameObject);
    }

}
