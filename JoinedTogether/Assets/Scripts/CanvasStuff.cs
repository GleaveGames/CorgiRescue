using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class CanvasStuff : MonoBehaviour
{
    Animator ani;
    private void Start()
    {
        ani = GetComponent<Animator>();
    }


    public void TimeToOne() 
    {
        Time.timeScale = 1;
    }
    public void TimeToLow() 
    {
        Time.timeScale = 0.1f;
    }

    public void LoadNextScene() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (transform.Find("Menu").gameObject.activeSelf)
            {
                ani.Play("CloseMenu");
            }
            else 
            {
                ani.Play("OpenMenu");
            }
        }
    }

    public void ResumeGame() 
    {
        ani.Play("CloseMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
