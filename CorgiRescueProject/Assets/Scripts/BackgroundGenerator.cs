using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundGenerator : MonoBehaviour
{
    [SerializeField]
    private Sprite[] bigbackgrounds;
    [SerializeField]
    private int sortinglayer = -20;
    [SerializeField]
    Material mat;

    private void Start()
    {
        GameObject backdaddy = new GameObject("Backgrounds");
        Vector2 startpos = transform.position;
        startpos.y -= 9;
        for (int i = 0; i < 12; i++) 
        {
            for(int j = 0; j < 15; j++) 
            {
                int rand = Random.Range(0, bigbackgrounds.Length);
                GameObject newsprite = new GameObject(bigbackgrounds[rand].name);
                newsprite.transform.position = new Vector2(startpos.x + i * 3, startpos.y + j * 3);
                newsprite.AddComponent<SpriteRenderer>().sprite = bigbackgrounds[rand];
                newsprite.GetComponent<SpriteRenderer>().sortingOrder = sortinglayer;
                newsprite.GetComponent<SpriteRenderer>().material = mat;
                newsprite.transform.parent = backdaddy.transform;
            }
        }
    }
}
