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
    public int initialHealth;
    [SerializeField]
    Sprite deadSprite;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
        int temp = health;
        initialHealth = temp;
        if (deadSprite == null) deadSprite = GetComponent<SpriteRenderer>().sprite;
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
        if (isServer && TryGetComponent(out Troop troop)) troop.StopAllCoroutines();
        gm.teams[team].things.Remove(gameObject);
        if (isServer) ClientRemove();
        if (occupiesTile)
        {
            Vector2Int tile = GetCurrentTile();
            gm.tiles[tile.x, tile.y] = 1;
        }
        Color temp = GetComponent<SpriteRenderer>().color;
        GetComponent<SpriteRenderer>().sprite = deadSprite;
        if(isServer) ClientSpriteChange();
        while (temp.a > 0.2) 
        {
            temp.a -= 0.01f;
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
        UpdateClientHealth();
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

    [ClientRpc]
    public void UpdateClientHealthPriv(int serverhealth) 
    {
        health = serverhealth;
    }

    public void UpdateClientHealth() 
    {
        if(isServer) UpdateClientHealthPriv(health);
    }

    [ClientRpc]
    void ClientSpriteChange() 
    {
        GetComponent<SpriteRenderer>().sprite = deadSprite;
    }
}
