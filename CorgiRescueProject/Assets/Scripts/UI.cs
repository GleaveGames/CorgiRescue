using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UI : MonoBehaviour
{
    public Text money;
    public Text health;
    public Text bombs;
    public Text level;
    public playerStats ps;
    public GameObject timerText;
    private Text texTime;
    public float timer;
    [Range(0.0f, 300.0f)]
    public float timerinit;
    [SerializeField]
    private bool unleashed = false;
    private GameObject map;
    private PlayerControls pc;
    [SerializeField]
    private GameObject[] menu;
    public AudioManager am;

    private void Awake()
    {
        pc = new PlayerControls();
        pc.Game.Menu.performed += _ => MenuToggle();
    }
    private void Start()
    {
        unleashed = false;
        texTime = timerText.GetComponent<Text>();
        timer = timerinit;
        texTime.text = Mathf.Round(timer).ToString();
        map = GameObject.FindGameObjectWithTag("Map");
        level.text = SceneManager.GetActiveScene().name;
    }

    // Update is called once per frame
    void Update()
    {
        if(ps == null)
        {
            ps = FindObjectOfType<playerStats>();
        }
        if (!unleashed)
        {
            money.text = ps.money.ToString();
            timer -= Time.deltaTime;
            texTime.text = Mathf.Round(timer).ToString();
            if (timer <= 0)
            {
                //Unleash the monster!
                map.GetComponent<LevelGenerator>().unleashCreature();
                texTime.text = "Unleashed!";
                texTime.color = Color.red;
                unleashed = true;
                am.PlayMusic("DirtThemeChase");
            }
        }
        health.text = ps.health.ToString();
        bombs.text = ps.bombs.ToString();
        if(ps.health <= 1)
        {
            health.color = Color.red;
        }
    }

    public void MenuToggle()
    {
        if (menu[0].activeSelf)
        {
            Time.timeScale = 1f;
            foreach (GameObject go in menu)
            {
                go.SetActive(false);
            }
        }
        else
        {
            Time.timeScale = 0f;
            foreach (GameObject go in menu)
            {
                go.SetActive(true);
            }
        }
    }

    private void OnEnable()
    {
        pc.Enable();
    }
    private void OnDisable()
    {
        pc.Disable();
    }
}

    
