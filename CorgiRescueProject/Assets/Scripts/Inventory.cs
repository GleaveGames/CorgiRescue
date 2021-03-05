using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private GameObject image;
    private float numItems;
    private playerStats ps;
    Coroutine coroutine;

    private void Start()
    {
        StartCoroutine("InventoryWait");   
    }

    public void NewItem(Sprite thissprite)
    {
        Vector3 pos = transform.position;
        pos.x = transform.position.x + numItems/0.02f;
        GameObject newimage = Instantiate(image, pos, Quaternion.identity);
        newimage.GetComponent<Image>().sprite = thissprite;
        newimage.transform.parent = transform;
        Vector3 newScale = newimage.transform.localScale;
        newScale = newScale / 2;
        newimage.transform.localScale = newScale;
        ps.inventory.Add(thissprite);
    }

    private void Update()
    {
        numItems = transform.childCount;
    }

    private IEnumerator InventoryWait()
    {
        yield return new WaitForSeconds(1);
        ps = FindObjectOfType<playerStats>();
        for (int i = 0; i < ps.inventory.Count; i++)
        {
            Sprite thissprite = ps.inventory[i];
            Vector3 pos = transform.position;
            pos.x = transform.position.x + numItems / 0.02f;
            GameObject newimage = Instantiate(image, pos, Quaternion.identity);
            newimage.GetComponent<Image>().sprite = thissprite;
            newimage.transform.parent = transform;
            Vector3 newScale = newimage.transform.localScale;
            newScale = newScale / 2;
            newimage.transform.localScale = newScale;
            numItems = transform.childCount;
        }
    }

}
