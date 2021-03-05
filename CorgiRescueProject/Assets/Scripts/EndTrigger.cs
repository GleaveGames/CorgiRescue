using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject EndAnimation;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            print("player");

            if(collision.gameObject.transform.GetChild(0).GetChild(1).Find("Pete") != null)
            {
                print("Pete");
                EndAnimation.SetActive(enabled);
            }
        }
    }
}
