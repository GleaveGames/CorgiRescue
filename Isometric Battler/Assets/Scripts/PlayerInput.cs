using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;

public class PlayerInput : NetworkBehaviour
{
    [SyncVar]
    public bool loaded;
    public GameObject build;
    GameManager gm;
    [SerializeField]
    Canvas uiCanvas;
    [SerializeField]
    Canvas canvas;
    [SyncVar]
    public int team;
    [SerializeField]
    GameObject ghostBuild;
    Coroutine coroutine;
    public int guild = 0;

    void Start()
    {
        if (isLocalPlayer)
        {
            gm = FindObjectOfType<GameManager>();
            gm.pi = this;
            GetComponent<SpriteRenderer>().color = gm.teams[team].color;
            FindObjectOfType<LobbySystem>().pi = this;
            uiCanvas = FindObjectOfType<Canvas>();
        }
        else 
        {
            gm = FindObjectOfType<GameManager>();
            GetComponent<SpriteRenderer>().color = gm.teams[team].color;
            this.enabled = false;
        }
    }

    public void TribeSelected(int tribe) 
    {
        guild = tribe;
        build = gm.guilds[guild].builds[0].build;
        StartCoroutine(GhostBuild());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLocalPlayer) return;
        else if (loaded)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 v3 = Input.mousePosition;
                v3.z = 10.0f;
                v3 = Camera.main.ScreenToWorldPoint(v3);
                gm.SpawnBuild(v3, build.name, team);
            }
        }
    }

    public void BasePlaced() 
    {
        if (isLocalPlayer) 
        {
            uiCanvas.transform.Find("Start").gameObject.SetActive(true);
            /*
            Instantiate(canvas);
            FindObjectOfType<BuildButtons>().pi = this;
            FindObjectOfType<BuildButtons>().guild = guild;
            basePlaced = true;
       */
            }
    }

    public IEnumerator StartCountdown() 
    {
        uiCanvas.transform.Find("Countdown").gameObject.SetActive(true);
        Text text = uiCanvas.transform.Find("Countdown").gameObject.GetComponent<Text>();
        text.text = "3";
        yield return new WaitForSeconds(0.5f);
        text.text = "2";
        yield return new WaitForSeconds(0.5f);
        text.text = "1";
        yield return new WaitForSeconds(0.5f);
        text.text = "Go!";
        StartGame();
        yield return new WaitForSeconds(1);
        text.gameObject.SetActive(false);
    }

    public void StartGame() 
    {
        Canvas can = Instantiate(canvas);
        can.transform.GetChild(0).GetComponent<Image>().color = gm.teams[team].color;
        FindObjectOfType<BuildButtons>().pi = this;
        FindObjectOfType<BuildButtons>().guild = guild;
    }

    public IEnumerator GhostBuild()
    {
        Vector3 v3 = Input.mousePosition;
        v3.z = 10.0f;
        v3 = Camera.main.ScreenToWorldPoint(v3);
        GameObject Ghost = Instantiate(ghostBuild, v3, Quaternion.identity);
        Ghost.GetComponent<SpriteRenderer>().sprite = build.GetComponent<SpriteRenderer>().sprite;
        while (loaded)
        {
            v3 = Input.mousePosition;
            v3.z = 10.0f;
            v3 = Camera.main.ScreenToWorldPoint(v3);
            float yRuff = 0.5f * (v3.y / 0.815f - v3.x / 1.415f + gm.boundsY - 1);
            float xRuff = v3.x / 1.4f + 0.5f * (v3.y / 0.815f - v3.x / 1.415f + gm.boundsY - 1);
            int y = Mathf.RoundToInt(yRuff);
            int x = Mathf.RoundToInt(xRuff);
            if (x > gm.boundsX-1)
            {
                x = gm.boundsX -1;
            }
            else if (x < 0) x = 0;
            if (y > gm.boundsY-1) y = gm.boundsY-1;
            else if (y < 0) y = 0;
            if (gm.tiles[x, y] == 1)
            {
                Vector3 spawn = transform.position;
                spawn.x = (x - y) * 1.415f;
                spawn.y = (x + y - gm.boundsY + 1) * 0.815f;
                Ghost.transform.position = spawn;
            }
            yield return null;
        }
        Destroy(Ghost);
    }
}