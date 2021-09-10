using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanPickUp : MonoBehaviour
{
    public List<GameObject> itemsForPickUp;
    public Transform leftHand;
    [SerializeField]
    private float pickUpRange;
    public GameObject item;
    public bool inBuyZone = false;
    public GameObject buyZoneItem;
    public LevelGenerator lg;
    private PlayerControls pc;
    private playerMovement pm;
    private AudioManager am;

    private void Awake()
    {
        pc = new PlayerControls();
        pc.Game.PickUp.performed += _ => PickUpPress();
        pc.Game.Fire.performed += _ => FirePress();
        am = FindObjectOfType<AudioManager>();
    }

    private void Start()
    {
        if (leftHand.childCount == 1)
        {
            item = transform.GetChild(0).gameObject;
        }
        lg = FindObjectOfType<LevelGenerator>();
        pm = GetComponent<playerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        itemsForPickUp = lg.itemsForPickUp;       
        if(leftHand.childCount < 1)
        {
            item = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ItemSlot")
        {
            buyZoneItem = collision.gameObject;
            inBuyZone = true;
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "ItemSlot")
        {
            buyZoneItem = null;  //not sure whether to get rid of this. If triggers overlap could be an issue if we delete this; 
            //could do something like check if this game object is the one that is the buyzoneitem and if it is then set it to null but idk
            inBuyZone = false;
        }
    }
    
    /*
    public void GetItemList()
    {
        itemsForPickUp.Clear();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("PickupItems"))
        {
            itemsForPickUp.Add(item);
        }
        Debug.Log("GotItems");
    }
    */

    public void Drop()
    {        
        if (item != null)
        {
            item.GetComponent<PickUpBase>().Drop();
        }                
    }
    
    public void PickUpPress()
    {
        if (pm.canMove)
        {
            if (inBuyZone)
            {
                if (buyZoneItem.GetComponent<ItemSlot>().thisItem.GetComponent<ShopInfo>().price <= FindObjectOfType<playerStats>().money)
                {
                    FindObjectOfType<playerStats>().money -= buyZoneItem.GetComponent<ItemSlot>().thisItem.GetComponent<ShopInfo>().price;
                    buyZoneItem.GetComponent<ItemSlot>().purchased();
                    am.Play("Pay", transform.position, true);
                }
                else
                {
                    am.Play("Denied", transform.position, false);
                }
            }
            else if (leftHand.childCount < 1)
            {
                if (itemsForPickUp[0] == null) { lg.itemsForPickUp.RemoveAt(0); }
                GameObject closestItem = itemsForPickUp[0];
                float closestDistance = Vector2.Distance(transform.position, itemsForPickUp[0].transform.position);
                for (int i = 1; i < itemsForPickUp.Count; i++)
                {
                    if (itemsForPickUp[i] != null)
                    {
                        if (Vector2.Distance(transform.position, itemsForPickUp[i].transform.position) < closestDistance)
                        {
                            closestDistance = Vector2.Distance(transform.position, itemsForPickUp[i].transform.position);
                            closestItem = itemsForPickUp[i];
                        }
                    }
                }
                //Debug.Log(closestDistance);
                if (closestDistance <= pickUpRange)
                {
                    closestItem.GetComponent<PickUpBase>().PickUp(leftHand);
                    item = closestItem;
                    Quaternion itemrot = Quaternion.LookRotation(transform.forward, transform.up);
                    item.transform.rotation = itemrot;
                }
                //Debug.Log(closestDistance);
            }
            else
            {
                if (item != null)
                {
                    item.GetComponent<PickUpBase>().Throw();
                }
            }
        }
        
    }

    private void FirePress()
    {
        if(leftHand.transform.childCount > 0)
        {
            if (pm.canMove)
            {
                if (item.TryGetComponent(out shotgun Gun))
                {
                    Gun.Fire();
                }
                else if (item.TryGetComponent(out Boomerang boom))
                {
                    boom.Fire();
                }
                else if(item.TryGetComponent(out Sword sword))
                {
                    sword.Fire();
                }
            }
        }
    }

    private void OnEnable()
    {
        pc.Enable();
    }
    private void OnDisable()
    {
        pc.Disable();
    }
}
