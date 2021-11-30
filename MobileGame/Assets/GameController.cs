using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject draggingObj;


    private void Update()
    {
        //stop dragging if mouse up
        if (Input.GetMouseButtonUp(0)) draggingObj = null;


        if(draggingObj != null)
        {
            draggingObj.transform.position = new Vector3(Camera.main.ScreenToWorldPoint(Input.mousePosition).x, Camera.main.ScreenToWorldPoint(Input.mousePosition).y, 0);
        }
    }







}
