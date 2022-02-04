using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;
    [HideInInspector]
    public Vector2 oppositePos;

    private void Start()
    {
        gameObject.SetActive(false);
        oppositePos = startPos;
        oppositePos.x = endPos.x - startPos.x;
    }

    


}
