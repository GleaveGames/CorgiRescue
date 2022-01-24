using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingText : MonoBehaviour
{
    public Vector2 startPos;
    public Vector2 endPos;

    private void Start()
    {
        gameObject.SetActive(false);
    }

}
