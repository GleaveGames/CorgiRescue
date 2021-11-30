using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject draggingObj;
    [SerializeField]
    LayerMask squares;

    

    private void Update()
    {
        
        if (draggingObj != null)
        {
            draggingObj.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
            
            
            
            //stop dragging if mouse up
            if (Input.GetMouseButtonUp(0))
            {
                Collider2D square = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), squares);
                if (square != null && !square.GetComponent<GameSquare>().occupied)
                {
                    draggingObj.transform.position = square.transform.position;
                    square.GetComponent<GameSquare>().occupied = true;
                    square.GetComponent<GameSquare>().occupier = draggingObj;

                    if (!draggingObj.GetComponent<ShopSprite>().beenPlaced) draggingObj.GetComponent<ShopSprite>().Bought();
                    draggingObj = null;
                }
                else
                {
                    draggingObj.transform.position = draggingObj.GetComponent<ShopSprite>().origin;
                    draggingObj = null;
                }
            }
        }
    }

    
}
