using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInput : NetworkBehaviour
{
    public bool loaded;
    public bool basePlaced;
    public GameObject build;
    GameManager gm;
    [SerializeField]
    Canvas canvas;
    public int team;
    [SerializeField]
    GameObject ghostBuild;
    Coroutine coroutine;
    


    // Start is called before the first frame update
    void Start()
    {
        if (isLocalPlayer)
        {
            gm = FindObjectOfType<GameManager>();
            gm.pi = this;
            //gotta assign the build as the base at the start
            build = gm.builds[0].build;
            StartCoroutine(GhostBuild());
            GetComponent<SpriteRenderer>().color = gm.teams[team].color;
        }
        else 
        {
            gm = FindObjectOfType<GameManager>();
            GetComponent<SpriteRenderer>().color = gm.teams[team].color;
            this.enabled = false;
        }
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
                gm.SpawnBush(v3, build.name, team);
            }
        }
    }

    public void BasePlaced() 
    {
        if (isLocalPlayer) 
        {
            Instantiate(canvas);
            FindObjectOfType<BuildButtons>().pi = this;
            basePlaced = true;
        }
    }

    public IEnumerator GhostBuild()
    {
        while (!gm.GameStarted) 
        {
            yield return null;
        }
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