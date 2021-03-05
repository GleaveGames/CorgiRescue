using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    [SerializeField]
    private GameObject[] itemsForSale;
    [SerializeField]
    private GameObject SK;
    public bool allPurchased;

    private void Update()
    {
        if (!allPurchased)
        {
            if (SK == null)
            {
                for (int i = 0; i < itemsForSale.Length; i++)
                {
                    if (itemsForSale[i] != null)
                    {
                        itemsForSale[i].GetComponent<ItemSlot>().purchased();
                    }
                    //should make it so all items act like that but the passive ones you just run into and then you get them
                }
                allPurchased = true;
                FindObjectOfType<playerStats>().wanted = true;
                FindObjectOfType<UI>().timer = 0;
            }
        }        
    }
}
