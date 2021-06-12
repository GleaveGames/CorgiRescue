using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    GameObject canvas;
    [SerializeField]
    Camera cam;
    [SerializeField]
    float levelTime;
    [SerializeField]
    AnimationCurve darknessJuice;
    [SerializeField]
    Color White;
    [SerializeField]
    Color Night;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        StartCoroutine(LevelTimer());
        canvas.SetActive(true);
        Time.timeScale = 0.05f;
    }

    public void LoadNextLevel()
    {
        canvas.GetComponent<Animator>().Play("CanvasEnd");
    }

    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    private IEnumerator LevelTimer() 
    {
        float counter = 0;
        while(counter < levelTime) 
        {
            cam.backgroundColor = Color.Lerp(White, Night, darknessJuice.Evaluate(counter / levelTime));
            counter += Time.deltaTime;
            yield return null;
        }
        LoadNextLevel();
    }
}
