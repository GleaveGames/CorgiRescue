using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlippyIce : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<playerMovement>().IceSlip = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<playerMovement>().IceSlip = false;
        }
    }
}
