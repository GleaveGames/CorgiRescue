using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public List<GameObject> ShopSlots;
    public List<GameObject> Units;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++) {
            ShopSlots.Add(transform.GetChild(i).gameObject);
        }
    }

    private void Update()
    {
        for (int i = 0; i < ShopSlots.Count; i++) {
            if (ShopSlots[i].transform.childCount < 1)
            {
                GameObject unit = Instantiate(Units[Random.Range(0, Units.Count)], ShopSlots[i].transform.position, Quaternion.identity);
                unit.transform.parent = ShopSlots[i].transform;
            }
        }
    }
}
