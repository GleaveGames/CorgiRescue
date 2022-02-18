using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;


public class MenuInputs : MonoBehaviour
{
    [SerializeField]
    Button continueButton;
    [SerializeField]
    Button newGameButton;
    [SerializeField]
    Text trophyCount;
    [SerializeField]
    TextMeshProUGUI usernameText;
    [SerializeField]
    Button logOut;
    [SerializeField]
    GameObject LoginGO; 
    [SerializeField]
    Button Quit;
    EventSystem system;

    private void Start()
    {
        system = EventSystem.current;
        continueButton.onClick.AddListener(() => {
            Continue();
        });
        newGameButton.onClick.AddListener(() => {
            NewGame();
        });
        Quit.onClick.AddListener(() => {
            QuitGame();
        });
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OnLogin()
    {
        if (MainMenu.Instance.inGame) continueButton.interactable = true;
        else continueButton.interactable = false;
        logOut.interactable = true;
        trophyCount.gameObject.SetActive(true);
        trophyCount.text = MainMenu.Instance.trophies.ToString();
        usernameText.gameObject.transform.parent.GetComponent<Animator>().enabled = true;
        usernameText.text = MainMenu.Instance.username;
        Quit.interactable = true;
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

    public void LogOut()
    {
        LoginGO.SetActive(true);
        trophyCount.gameObject.SetActive(false);
        logOut.interactable = false;
        MainMenu.Instance.ResetClientStats();
        StartCoroutine(ReFurlUsername());
    }

    private IEnumerator ReFurlUsername()
    {
        usernameText.gameObject.transform.parent.GetComponent<Animator>().Play("BasicBannerRefurl");
        yield return new WaitForSeconds(0.9f);
        usernameText.gameObject.transform.parent.GetComponent<Animator>().Play("BasicBanner");
        usernameText.gameObject.transform.parent.GetComponent<Animator>().enabled = false;
        usernameText.text = "";
        gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Selectable next = system.currentSelectedGameObject.GetComponent<Selectable>().FindSelectableOnDown();

            if (next != null)
            {

                InputField inputfield = next.GetComponent<InputField>();
                if (inputfield != null)
                    inputfield.OnPointerClick(new PointerEventData(system));  //if it's an input field, also set the text caret

                system.SetSelectedGameObject(next.gameObject, new BaseEventData(system));
            }
            //else Debug.Log("next nagivation element not found");

        }
    }
}

