using UnityEngine;
using UnityEngine.SceneManagement;

public class endHole : MonoBehaviour
{
    private playerStats ps;
    private GameObject cam;
    public bool LoadTriggered;
    private bool loadScene;
    private GameObject player;
    private Vector3 initialScale;
    [SerializeField]
    private GameObject black;
    private float timer;

    private void Start()
    {
        ps = FindObjectOfType<playerStats>();
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        player = GameObject.FindGameObjectWithTag("Player");
        initialScale = player.transform.localScale;
        black = GameObject.FindGameObjectWithTag("Black");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {            
            DontDestroyOnLoad(collision.gameObject);
            ps.holdItem = collision.gameObject.GetComponent<CanPickUp>().item;
            LoadTriggered = true;
            player.GetComponent<playerMovement>().canMove = false;
            player.GetComponent<Rigidbody2D>().isKinematic = true;
            player.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    private void FixedUpdate()
    {
        //RandomBarkingSounds
        if(timer < 0)
        {
            GetComponent<AudioSource>().Play();
            GetComponent<AudioSource>().pitch = Random.Range(1f, 1.3f);
            timer = 5 + Random.Range(1, 10);
        }
        else
        {
            timer -= Time.deltaTime;
        }



        if (LoadTriggered)
        {
            black.GetComponent<Animator>().Play("BlackFadeIn");
            player.GetComponent<CircleCollider2D>().enabled = false;
            // maybe make the player move towards the hole in exponential  decay
            player.transform.position = Vector2.MoveTowards(player.transform.position, transform.position, 1*Time.deltaTime);
            Vector3 scale = player.transform.localScale;
            scale.x = Mathf.Lerp(scale.x, 0, 0.04f);
            scale.y = Mathf.Lerp(scale.y, 0, 0.04f);
            player.transform.localScale = scale;
            cam.GetComponent<Camera>().orthographicSize = Mathf.MoveTowards(cam.GetComponent<Camera>().orthographicSize, 0.2f, 0.05f);
            if(scale.x < 0.01)
            {
                loadScene = true;
                LoadTriggered = false;
            }
        }

        if (loadScene)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
