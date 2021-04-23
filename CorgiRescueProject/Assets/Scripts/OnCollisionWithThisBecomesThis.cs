using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnCollisionWithThisBecomesThis : MonoBehaviour
{
    [SerializeField]
    private string thingThatBreaksThis;
    [SerializeField]
    private GameObject thingThatThisTurnsInto;
    [SerializeField]
    private string sound;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name.Contains(thingThatBreaksThis))
        {
            Destroy(gameObject);
            Instantiate(thingThatThisTurnsInto, transform.position, Quaternion.identity);
            if(sound != null)
            {
                FindObjectOfType<AudioManager>().Play(sound, transform.position, true);
            }
            //instantiate eggshells
        }

    }

}
