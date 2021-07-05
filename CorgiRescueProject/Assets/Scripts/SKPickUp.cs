using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKPickUp : MonoBehaviour
{
    public List<GameObject> itemsForPickUp;
    public Transform leftHand;
    public float pickUpRange;
    public GameObject item;
    private SKMovement skm;
    public float viewRange;

    private void Start()
    {
        skm = GetComponent<SKMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (itemsForPickUp.Count == 0)
        {
            itemsForPickUp = FindObjectOfType<LevelGenerator>().itemsForPickUp;
        }
        if(leftHand.childCount < 1)
        {
            item = null;
        }
    }

    public GameObject GetItem()
    {
        item = null;
        GameObject[] closestItems = GetNearestItem();
        if(closestItems != null)
        {
            for (int i = 0; i < closestItems.Length; i++)
            {
                float TargetDistance = 999;
                float WallDistance = 998;
                bool HitTarget = false;
                RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, closestItems[i].transform.position - transform.position, viewRange);
                for (int j = 0; j < hit.Length; j++)
                {
                    //check if hit
                    if (hit[j].collider.gameObject == closestItems[i])
                    {
                        HitTarget = true;
                        TargetDistance = Vector2.Distance(transform.position, hit[j].point);
                    }
                    else if (hit[j].collider.gameObject.CompareTag("Wall"))
                    {
                        if (WallDistance > Vector2.Distance(transform.position, hit[j].point))
                        {
                            WallDistance = Vector2.Distance(transform.position, hit[j].point);
                        }
                    }
                    else if (hit[j].collider.gameObject.CompareTag("Rock"))
                    {
                        if (WallDistance > Vector2.Distance(transform.position, hit[j].point))
                        {
                            WallDistance = Vector2.Distance(transform.position, hit[j].point);
                        }
                    }
                    else if (hit[j].collider.name == "Obsidian")
                    {
                        if (WallDistance > Vector2.Distance(transform.position, hit[j].point))
                        {
                            WallDistance = Vector2.Distance(transform.position, hit[j].point);
                        }
                    }
                }
                if (HitTarget)
                {
                    if (TargetDistance < WallDistance)
                    {
                        item = closestItems[i];
                        return item;
                    }
                }
            }
            return item;
        }
        else
        {
            return null;
        }
    }


    public GameObject[] GetNearestItem()
    {
        if (itemsForPickUp.Count > 0)
        {
            GameObject closestItem = itemsForPickUp[0];
            GameObject closestItem2 = itemsForPickUp[0];
            GameObject closestItem3 = itemsForPickUp[0];
            float closestDistance = 99;
            if (itemsForPickUp[0] != null) { closestDistance = Vector2.Distance(transform.position, itemsForPickUp[0].transform.position); }
            for (int i = 1; i < itemsForPickUp.Count; i++)
            {
                if(itemsForPickUp[i] != null)
                {
                    if (Vector2.Distance(transform.position, itemsForPickUp[i].transform.position) < closestDistance)
                    {
                        closestDistance = Vector2.Distance(transform.position, itemsForPickUp[i].transform.position);
                        closestItem3 = closestItem2;
                        closestItem2 = closestItem;
                        closestItem = itemsForPickUp[i];
                    }
                }
            }
            GameObject[] three = new GameObject[3];
            three[0] = closestItem;
            three[1] = closestItem2;
            three[2] = closestItem3;
            //Debug.Log(three[0].name + three[1].name + three[2].name);
            return three;
        }
        else
        {
            return null;
        }
    }
    public void Drop()
    {
        if (item != null)
        {
            item.GetComponent<PickUpBase>().Drop();
        }
    }

    public void ThrowItem()
    {
        item.GetComponent<PickUpBase>().Throw();
    }
}
