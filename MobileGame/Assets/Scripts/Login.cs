using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Login : MonoBehaviour
{
    public InputField UsernameInput;
    public InputField PasswordInput;
    public Button LoginButton;
    public Button NewAccountButton;
    public Button PlayGuestButton;
    [SerializeField]
    TextMeshProUGUI errorText;
    EventSystem system;
    [SerializeField]
    GameObject MenuGO;

    private void Start()
    {
        system = EventSystem.current;
        LoginButton.onClick.AddListener(() => {
            LoginTrigger();
        });
        NewAccountButton.onClick.AddListener(() => {
            StartCoroutine(MainMenu.Instance.db.RegisterUser(UsernameInput.text, PasswordInput.text));
        }); 
        PlayGuestButton.onClick.AddListener(() => {
            GuestTrigger();
        });
        StartCoroutine(LateStart());
    }

    void LoginTrigger()
    {
        MenuGO.SetActive(true);
        StartCoroutine(MainMenu.Instance.db.Login(UsernameInput.text, PasswordInput.text));
    }

    void GuestTrigger()
    {
        MenuGO.SetActive(true);
        string tempUsername = "Guest " + System.DateTime.Now;
        tempUsername.Replace('/', ' ');
        tempUsername.Replace('\\', ' ');
        StartCoroutine(MainMenu.Instance.db.RegisterUser(tempUsername, ""));
    }


    IEnumerator LateStart()
    {
        yield return new WaitForEndOfFrame();
        if (PlayerPrefs.GetString("Username") != null)
        {
            MenuGO.SetActive(true);
            StartCoroutine(MainMenu.Instance.db.Login(PlayerPrefs.GetString("Username"), PlayerPrefs.GetString("Password")));
        }
    }


    public void LoginSuccess()
    {
        MenuGO.SetActive(true);
        PlayerPrefs.SetString("Username", MainMenu.Instance.username);
        PlayerPrefs.SetString("Password", MainMenu.Instance.password);
        gameObject.SetActive(false);
    }

    public IEnumerator DisplayText(string error)
    {
        MenuGO.SetActive(false);
        errorText.text = error;
        yield return new WaitForSeconds(3);
        errorText.text = "";
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
