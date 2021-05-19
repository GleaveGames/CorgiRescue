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

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();
    }


    private void Update()
    {
        if (!dead) 
        {
            if (health <= 0)
            {
                dead = true;
                StartCoroutine(Die());
                if (transform.gameObject.name == "Base") 
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
            ClientDie(temp);
            yield return null;
        }
        Destroy(gameObject);
    }

    [ClientRpc]
    private void ClientDie(Color temp)
    {
        GetComponent<SpriteRenderer>().color = temp;
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
