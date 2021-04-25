using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Web : MonoBehaviour
{
    GameObject Player;
    SpriteRenderer sprite;
    [SerializeField]
    private float WebLoss;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.transform.TryGetComponent(out PickUpItem pickup))
        {
            pickup.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
        }
        if (collision.gameObject == Player) 
        {
            if(Player.GetComponent<Rigidbody2D>().velocity.magnitude > 0.01f) 
            {
                Color tmp = sprite.color;
                tmp.a-=WebLoss;
                sprite.color = tmp;
                Debug.Log(tmp.a);
                if(tmp.a <= 0.02f) 
                {
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Player = collision.gameObject;
            Player.GetComponent<playerMovement>().runsp = FindObjectOfType<playerStats>().moveSpeed / 6;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player")) 
        {
            Player.GetComponent<playerMovement>().runsp = FindObjectOfType<playerStats>().moveSpeed;
        }
    }
}
