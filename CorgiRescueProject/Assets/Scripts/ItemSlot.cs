using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField]
    private int price;
    [SerializeField]
    private Text priceText;
    public GameObject thisItem;
    private playerStats ps;
    private GameObject player;
    private GameObject map;
    public bool passive;
    // Start is called before the first frame update
    void Start()
    {
        ps = FindObjectOfType<playerStats>();
        thisItem = ps.itemsForSale[Random.Range(0, ps.itemsForSale.Count)];
        GetComponent<SpriteRenderer>().sprite = thisItem.GetComponent<SpriteRenderer>().sprite;
        price = thisItem.GetComponent<ShopInfo>().price;
        priceText.text = price.ToString();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void purchased()
    {
        if (!thisItem.GetComponent<ShopInfo>().passive)
        {
            GameObject item = Instantiate(thisItem, transform.position, Quaternion.identity);
            player.GetComponent<CanPickUp>().itemsForPickUp.Add(item);
        }
        else
        {
            GameObject item = Instantiate(thisItem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
