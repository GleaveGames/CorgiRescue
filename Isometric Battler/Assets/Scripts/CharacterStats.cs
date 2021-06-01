using Mirror;
using System.Collections;
using UnityEngine;

public class CharacterStats : NetworkBehaviour
{
    public int health;
    [SyncVar]
    public int team;
    Coroutine coroutine;
    bool dead;
    [SerializeField]
    bool occupiesTile;
    GameManager gm;
    Vector2 tile;
    int initialHealth;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        initialHealth = health;
    }


    private void Update()
    {
        if (!dead) 
        {
            if (health <= 0)
            {
                dead = true;
                StartCoroutine(Die());
                if (transform.gameObject.name.Contains("Base")) 
                {
                    gm.EndGame();
                }
            }
        }
    }

    private IEnumerator Die() 
    {
        if (TryGetComponent(out Spawner spawn))
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).TryGetComponent(out CharacterStats cs)) transform.GetChild(i).transform.parent = null;
            }
        }
        gm.teams[team].things.Remove(gameObject);
        if (isServer) ClientRemove();
        if (occupiesTile)
        {
            Vector2Int tile = GetCurrentTile();
            gm.tiles[tile.x, tile.y] = 1;
        }
        Color temp = GetComponent<SpriteRenderer>().color;
        while (temp.a > 0.2) 
        {
            temp.a -= 0.08f;
            GetComponent<SpriteRenderer>().color = temp;
            if (isServer) ClientFade(temp);
            yield return null;
        }
        Destroy(gameObject);
    }

    public void Convert(int newTeam)
    {
        health = initialHealth;
        gm.teams[team].things.Remove(gameObject);
        gm.teams[newTeam].things.Add(gameObject);
        GetComponent<SpriteRenderer>().color = gm.teams[newTeam].color;
        if (isServer) ClientConvert(newTeam);
    }


    [ClientRpc]
    private void ClientConvert(int newTeam)
    {
        gm.teams[team].things.Remove(gameObject);
        gm.teams[newTeam].things.Add(gameObject);
        GetComponent<SpriteRenderer>().color = gm.teams[newTeam].color;
    }


    [ClientRpc]
    private void ClientFade(Color temp)
    {
        GetComponent<SpriteRenderer>().color = temp;
    }
    
    [ClientRpc]
    private void ClientRemove()
    {
        gm.teams[team].things.Remove(gameObject);
    }


    private Vector2Int GetCurrentTile()
    {
        float yRuff = 0.5f * (transform.position.y / 0.815f - transform.position.x / 1.415f + gm.boundsY - 1);
        float xRuff = transform.position.x / 1.4f + 0.5f * (transform.position.y / 0.815f - transform.position.x / 1.415f + gm.boundsY - 1);
        int y = Mathf.RoundToInt(yRuff);
        int x = Mathf.RoundToInt(xRuff);
        return new Vector2Int(x, y);
    }
}
