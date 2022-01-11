using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverlayCanvas : MonoBehaviour
{
    [SerializeField]
    GameObject clouds;
    GameObject cloudsObj;
    private Vector2 res;
    void Start()
    {
        GameObject cloudsObj = Instantiate(clouds, transform.position, Quaternion.identity);
        cloudsObj.transform.parent = transform;
        cloudsObj.transform.SetSiblingIndex(0);
        res = new Vector2(Screen.width, Screen.height);
    }

    void Update()
    {
        Vector2 newRes = new Vector2(Screen.width, Screen.height);
        if (res != newRes)
        {
            Destroy(cloudsObj);
            cloudsObj = Instantiate(clouds, transform.position, Quaternion.identity);
            cloudsObj.transform.parent = transform;
            res = newRes;
        }
    }
}
