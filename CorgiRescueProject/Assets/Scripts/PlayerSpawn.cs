using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            SceneManager.MoveGameObjectToScene(GameObject.FindGameObjectWithTag("Player"), SceneManager.GetActiveScene());
            player.transform.position = transform.position;
        }
        else
        {
            player = Instantiate(player, transform.position, Quaternion.identity);
        }
        player.GetComponent<CanPickUp>().lg = FindObjectOfType<LevelGenerator>();
        player.GetComponent<CanPickUp>().itemsForPickUp = new List<GameObject>();
        player.GetComponent<playerMovement>().canMove = true;
        player.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
        player.GetComponent<Rigidbody2D>().isKinematic = false;
        player.GetComponent<CircleCollider2D>().enabled = true;
    }
}
