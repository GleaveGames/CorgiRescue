using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerStats : MonoBehaviour
{
    public int money = 0;
    public int health = 5;
    public int bombs = 5;
    public float miningSpeed;
    public float moveSpeed;
    public GameObject holdItem;
    private GameObject player;
    public static playerStats instance;
    public bool spikegloves = false;
    public bool steeltoes = false;
    public bool bigbombs = false;
    public List<GameObject> itemsForSale;
    private List<GameObject> itemsForSaleInitial;
    public bool glasses = false;
    [SerializeField]
    public List<Sprite> inventory;
    public bool wanted = false;



    // Start is called before the first frame update
    void Start()
    {
        itemsForSaleInitial = itemsForSale;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        //player = GameObject.FindGameObjectWithTag("Player");
        //Instantiate(holdItem, player.transform.GetChild(0).GetChild(2).position, Quaternion.identity);
    }

    private void Update()
    {
        if(health < 0)
        {
            health = 0;
        }
    }

    public void ResetStats()
    {
        money = 0;
        health = 5;
        bombs = 5;
        miningSpeed = 1;
        moveSpeed = 6;
        spikegloves = false;
        steeltoes = false;
        bigbombs = false;
        itemsForSale = itemsForSaleInitial;
        glasses = false;
        wanted = false;
        inventory = new List<Sprite>();
    }
}
