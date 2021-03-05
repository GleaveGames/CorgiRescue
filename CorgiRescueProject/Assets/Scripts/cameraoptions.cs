using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraoptions : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool follow = false;
    public float goalsize;
    public bool changesize = false;
    public Vector3 goalPos;
    [SerializeField]
    private PlayerControls pc;
    private bool look;
    public bool boss;
    public Transform bosspos;

    [Header ("SHAKE")]
    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;

    // How long the object should shake for.
    public float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public float shakeAmount = 0.1f;
    public float decreaseFactor = 1f;
    Vector3 originalPos;
    void Awake()
    {
        camTransform = transform;
        pc = new PlayerControls();
        pc.Game.Look.started += _ => Look();
        pc.Game.Look.canceled += _ => StopLook();
    }
    void OnEnable()
    {
        originalPos = camTransform.localPosition;
        pc.Enable();
    }


    private void Start()
    {
        Vector3 goalpos = transform.position;        
    }

    void Update()
    {
        if (!look)
        {
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            if (!boss)
            {
                goalPos = player.position;
                goalPos.z = transform.position.z;
            }
            else
            {
                if(bosspos == null)
                {
                    bosspos = player;
                }
                goalPos = (player.position + bosspos.position) / 2;
                goalPos.z = transform.position.z;
            }
            
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
            originalPos = transform.position;
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
            }
        }
        else
        {
            Vector2 move = pc.Game.Move.ReadValue<Vector2>();
            if (player == null)
            {
                player = GameObject.FindGameObjectWithTag("Player").transform;
            }
            goalPos = player.position;
            goalPos.z = transform.position.z;
            goalPos.x = player.position.x + move.x * 6;
            goalPos.y = player.position.y + move.y * 6;
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref velocity, smoothTime);
            originalPos = transform.position;
            if (shakeDuration > 0)
            {
                camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;
                shakeDuration -= Time.deltaTime * decreaseFactor;
            }
            else
            {
                shakeDuration = 0f;
            }
        }
        
    }

    private void Look()
    {
        look = true;
        player.GetComponent<playerMovement>().canMove = false;
    }

    private void StopLook()
    {
        look = false;
        player.GetComponent<playerMovement>().canMove = true;
    }

    private void OnDisable()
    {
        pc.Disable();
    }

}
