using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSprite : MonoBehaviour
{
    GameController gc;
    public Vector3 origin;
    [SerializeField]
    LayerMask squares;
    public bool beenPlaced;

    private void Start()
    {
        gc = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            origin = transform.position;
            Collider2D targetObject = Physics2D.OverlapPoint(mousePosition);
            if (targetObject && targetObject == this.gameObject.GetComponent<Collider2D>())
            {
                gc.draggingObj = this.gameObject;
                Collider2D square = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), squares);
                
                //Already On Board
                if(square != null)
                {
                    square.GetComponent<GameSquare>().occupied = false;
                    square.GetComponent<GameSquare>().occupier = null;
                }
            }
        }
    }

    public void Bought()
    {
        beenPlaced = true;
        transform.parent = transform.root;
        StartCoroutine(GetComponent<Unit>().OnBuy());
    }
}
