using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSquare : MonoBehaviour
{
    public bool occupied;
    public GameObject occupier;
    [SerializeField]
    Sprite[] buffs;
    bool showingBuff;
    AnimationCurve gsjuice;
    Color invis;

    private void Start()
    {
        gsjuice = FindObjectOfType<GameController>().gsjuice;
        invis = Color.white;
        invis.a = 0;
    }

    public IEnumerator showBuff(int buff)
    {
        showingBuff = true;
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = buffs[buff];
        float timer = 0;
        while(timer < 1)
        {
            transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(invis, Color.white, gsjuice.Evaluate(timer));
            timer += Time.deltaTime;
            yield return null;
        }
        showingBuff = false;
    }
}
