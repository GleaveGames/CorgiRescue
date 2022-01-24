using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuInputs : MonoBehaviour
{
    [SerializeField]
    Button continueButton;
    [SerializeField]
    Button newGameButton;
    [SerializeField]
    Text trophyCount;
    [SerializeField]
    Text usernameText;


    private void Start()
    {
        continueButton.onClick.AddListener(() => {
            Continue();
        });
        newGameButton.onClick.AddListener(() => {
            NewGame();
        });
    }


    public void OnLogin()
    {
        if (MainMenu.Instance.inGame)
        {
            continueButton.interactable = true;
        }
        trophyCount.text = MainMenu.Instance.trophies.ToString();
        usernameText.text = MainMenu.Instance.username;
    }

    public void Continue()
    {
        //continue stuff;
        MainMenu.Instance.continuing = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
