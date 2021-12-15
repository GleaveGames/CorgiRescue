using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<ShopSpot> ShopSlots;
    public List<GameObject> Units;
    GameController gc;

    private void Start()
    {
        ShopSlots = new List<ShopSpot>();
        gc = FindObjectOfType<GameController>();
        for (int i = 0; i < transform.childCount; i++) {
            ShopSpot newSpot = new ShopSpot();
            ShopSlots.Add(newSpot);
            newSpot.go = transform.GetChild(i).gameObject;
            newSpot.frozen = false;
            newSpot.sr = newSpot.go.transform.GetChild(1).GetComponent<SpriteRenderer>();
            newSpot.number = i;
        }
        ReRoll();
        gc.Gold++;
    }

    private void Update()
    {
        foreach(ShopSpot thisSpot in ShopSlots)
        {
            if (thisSpot.frozen) thisSpot.sr.enabled = true;
            else thisSpot.sr.enabled = false;
        }
    }

    public void ReRoll()
    {
        if (gc.Gold > 0)
        {
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].frozen) continue;
                if (ShopSlots[i].go.transform.childCount > 2)
                {
                    Destroy(ShopSlots[i].go.transform.GetChild(2).gameObject);
                }
                GameObject unit = Instantiate(Units[Random.Range(0, Units.Count)], ShopSlots[i].go.transform.position, Quaternion.identity);
                unit.transform.parent = ShopSlots[i].go.transform;
            }
            gc.Gold--;
        }
        else
        {
            StartCoroutine(gc.GoldJuice());
        }
    }

    public void ToggleFreeze(int spot)
    {
        ShopSpot sp = ShopSlots[spot];
        if(sp.go != null)
        {
            sp.frozen = !sp.frozen;
        }
        else
        {
            sp.frozen = false;
        }
    }
}

public class ShopSpot
{
    public GameObject go;
    public bool frozen;
    public SpriteRenderer sr;
    public int number;
}
