using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YetiBoss2 : MonoBehaviour
{

    public GameObject boss;
    public GameObject yetispawn;
    private GameObject yeti;
    private bool spawned;
    private Coroutine coroutine;
    private GameObject player;
    [SerializeField]
    private GameObject obsidianBlock;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!spawned)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                player = collision.gameObject;
                StartCoroutine("BossSequence");
            }
        }
    }

    private IEnumerator BossSequence()
    {
        player.GetComponent<playerMovement>().canMove = false;
        player.GetComponent<playerMovement>().ChangeAnimationState("Idle");
        RigidbodyConstraints2D constaints = player.GetComponent<Rigidbody2D>().constraints;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        yield return new WaitForSeconds(2f);
        yeti = Instantiate(boss, yetispawn.transform.position, yetispawn.transform.rotation);
        FindObjectOfType<cameraoptions>().bosspos = yeti.transform;
        FindObjectOfType<cameraoptions>().boss = true;
        spawned = true;
        obsidianBlock.SetActive(true);
        yield return new WaitForSeconds(1f);
        player.GetComponent<playerMovement>().canMove = true;
        player.GetComponent<Rigidbody2D>().constraints = constaints;
    }
}
