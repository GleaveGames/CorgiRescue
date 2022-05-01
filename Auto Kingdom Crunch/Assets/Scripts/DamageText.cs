using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageText : MonoBehaviour
{
    TextMeshPro sr;
    [SerializeField]
    float time;
    [SerializeField]
    AnimationCurve colorFade;
    Color invisColor;
    [SerializeField]
    AnimationCurve moveY;
    Color startColor;
    bool hasChild = false;
    SpriteRenderer childSR;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<TextMeshPro>();
        startColor = sr.color;
        invisColor = startColor;
        invisColor.a = 0;
        StartCoroutine(FloatUp());
        if (transform.childCount > 0) hasChild = true;
    }

    private IEnumerator FloatUp()
    {
        yield return new WaitForEndOfFrame();
        float timer = 0;
        Vector2 initpos = transform.position;
        //initpos.x -= 0.2f;
        //initpos.y += 0.15f;
        sr.sortingOrder = 1000;
        if (!hasChild)
        {
            while (timer < time)
            {
                sr.color = Color.Lerp(startColor, invisColor, colorFade.Evaluate(timer / time));
                transform.position = new Vector2(initpos.x, initpos.y + 0.4f * moveY.Evaluate(timer / time));
                timer += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            childSR = transform.GetChild(0).GetComponent<SpriteRenderer>();
            while (timer < time)
            {
                sr.color = Color.Lerp(startColor, invisColor, colorFade.Evaluate(timer / time));
                childSR.color = Color.Lerp(startColor, invisColor, colorFade.Evaluate(timer / time));
                transform.position = new Vector2(initpos.x, initpos.y + 0.4f * moveY.Evaluate(timer / time));
                timer += Time.deltaTime;
                yield return null;
            }
        }
        
        Destroy(gameObject);
    }
}
