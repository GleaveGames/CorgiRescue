using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassivePickup : MonoBehaviour
{
    private playerStats ps;
    private GameObject player;
    private AudioManager am;
    private GameObject invent;
    private void Start()
    {
        ps = FindObjectOfType<playerStats>();
        player = GameObject.FindGameObjectWithTag("Player");
        am = FindObjectOfType<AudioManager>();
        invent = GameObject.FindGameObjectWithTag("Inventory");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject == player)
        {
            if (gameObject.name.Contains("Money"))
            {
                if (gameObject.name == ("MoneyS(Clone)"))
                {
                    ps.money += 200;
                    am.Play("Silver", transform.position, false);
                }
                else if (gameObject.name == ("MoneyM(Clone)"))
                {
                    ps.money += 1000;
                    am.Play("Gold", transform.position, false);
                }
                else if (gameObject.name == ("MoneyL(Clone)"))
                {
                    ps.money += 3000;
                    am.Play("Diamond", transform.position, false);
                }
                else
                {
                    print("This money item isn't coded yet");
                }
            }
            else if (gameObject.name == "Snack(Clone)" || gameObject.name == "FriedEgg(Clone)")
            {
                ps.health += 1;
                am.Play("Snack", transform.position, true);
            }
            else if (gameObject.name == "BombBag(Clone)")
            {
                ps.bombs += 3;
                am.Play("PickUp", transform.position,true);
            }
            else if (gameObject.name == "BombBox(Clone)")
            {
                ps.bombs += 10;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }
            else if (gameObject.name == "GoldPick(Clone)")
            {
                RemoveFromList("GoldPick", ps.itemsForSale);
                ps.miningSpeed = 0.4f;
                player.GetComponent<playerMovement>().miningSpeed = 0.8f;
                //player.transform.GetChild(0).Find("pickaxe").gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }
            else if (gameObject.name == "DiamondPick(Clone)")
            {
                RemoveFromList("DiamondPick", ps.itemsForSale);
                RemoveFromList("GoldPick", ps.itemsForSale);
                ps.miningSpeed = 0.2f;
                player.GetComponent<playerMovement>().miningSpeed = 0.6f;
                //player.transform.GetChild(0).Find("pickaxe").gameObject.GetComponent<SpriteRenderer>().sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }
            else if (gameObject.name == "SpikeGloves(Clone)")
            {
                RemoveFromList("SpikeGloves", ps.itemsForSale);
                ps.spikegloves = true;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }
            else if (gameObject.name == "Glasses(Clone)")
            {
                ps.glasses = true;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
                ps.wanted = false;
            }
            else if(gameObject.name == "RunningShoes(Clone)")
            {
                RemoveFromList("RunningShoes", ps.itemsForSale);
                ps.moveSpeed = 8;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);

            }
            else if(gameObject.name == "SteelToeBoots(Clone)")
            {
                RemoveFromList("SteelToeBoots", ps.itemsForSale);
                ps.steeltoes = true;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }
            else if(gameObject.name == "BigBombs(Clone)")
            {
                RemoveFromList("BigBombs", ps.itemsForSale);
                ps.bigbombs = true;
                invent.GetComponent<Inventory>().NewItem(gameObject.GetComponent<SpriteRenderer>().sprite);
                am.Play("PickUp", transform.position, true);
            }

            Destroy(gameObject);
        }        
    }

    private void RemoveFromList(string name, List<GameObject> list)
    {
        bool removed = false;
        for (int i = 0; i < list.Count; i++)
        {
            if(list[i].name == name)
            {
                list.RemoveAt(i);
                removed = true;
            }
        }
        if (!removed)
        {
            print("Couldn't find in list to Remove");
        }
    }
}
