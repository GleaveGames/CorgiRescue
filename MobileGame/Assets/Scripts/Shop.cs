using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public List<GameObject> ShopSlots;
    public List<GameObject> Units;
    GameController gc;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
        for (int i = 0; i < transform.childCount; i++) {
            ShopSlots.Add(transform.GetChild(i).gameObject);
        }
        ReRoll();
        gc.Gold++;
    }

    public void ReRoll()
    {
        if (gc.Gold > 0)
        {
            for (int i = 0; i < ShopSlots.Count; i++)
            {
                if (ShopSlots[i].transform.childCount > 0)
                {
                    Destroy(ShopSlots[i].transform.GetChild(0).gameObject);
                }
                GameObject unit = Instantiate(Units[Random.Range(0, Units.Count)], ShopSlots[i].transform.position, Quaternion.identity);
                unit.transform.parent = ShopSlots[i].transform;
            }
            gc.Gold--;
        }
        else
        {
            StartCoroutine(gc.GoldJuice());
        }
    }
}
