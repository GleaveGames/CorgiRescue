using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private Animator black;
    [SerializeField]
    private GameObject[] menuObjs;
    [SerializeField]
    private bool menu;
    [SerializeField]
    private bool gameComplete = false;


    private void Start()
    {
        if (menu)
        {
            FindObjectOfType<AudioManager>().PlayMusic("MenuTheme");
        }
    }

    public void LoadGame()
    {
        if (!gameComplete)
        {
            if (!menu)
            {
                FindObjectOfType<playerStats>().ResetStats();
                Time.timeScale = 1f;
            }
            SceneManager.LoadScene(1);
        }
        else
        {
            Time.timeScale = 0f;
            SceneManager.LoadScene(0);
        }
    }   

    public void QuitGame()
    {
        Application.Quit();
    }

    public void Fade()
    {
        //just reload agian for now
        if (menuObjs.Length>0)
        {
            Time.timeScale = 0f;
            foreach (GameObject obj in menuObjs)
            {
                obj.SetActive(true);
            }
        }
        if (menu)
        {
            black.Play("BlackFadeInMenu");
        }
        else
        {
            black.Play("BlackFadeIn");
        }
    }
}
