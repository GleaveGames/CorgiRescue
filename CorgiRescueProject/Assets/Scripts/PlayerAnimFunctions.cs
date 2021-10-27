using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerAnimFunctions : MonoBehaviour
{
    [SerializeField]
    private Collider2D coll;


    public void enableCollider()
    {
        coll.enabled = true;
        //transform.parent.GetComponent<playerMovement>().canMine = false;

    }
    public void disableCollider()
    {
        coll.enabled = false;
    }

    public void Restart()
    {
        FindObjectOfType<playerStats>().health = 5;
        FindObjectOfType<playerStats>().money = 0;
        Destroy(transform.parent.gameObject);
        SceneManager.LoadScene(0);
    }

    public void mineEnd()
    {
        transform.parent.GetComponent<playerMovement>().mining = false;
    }


    public void TriggerCanMine()
    {
        transform.Find("pickaxe").GetComponent<PickAxe>().canMine = true;
    }
}
