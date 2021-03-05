using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velocityDrag : MonoBehaviour
{
    private Rigidbody2D rb;
    
    [Header("Below this speed")]
    [SerializeField]
    private float velocityCutoff;
    [Header("The Linear Drag is this --")]
    [SerializeField]
    private float lowVDrag;
    [Header("Else it's this --")]
    [SerializeField]
    private float highVDrag;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (rb.velocity.magnitude > velocityCutoff)
        {
            rb.drag = highVDrag;
        }
        else
        {
            rb.drag = lowVDrag;
        }
    }
}
