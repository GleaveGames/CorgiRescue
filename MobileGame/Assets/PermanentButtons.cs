using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PermanentButtons : MonoBehaviour
{
    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}
