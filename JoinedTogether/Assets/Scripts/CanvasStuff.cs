using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CanvasStuff : MonoBehaviour
{
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
}
