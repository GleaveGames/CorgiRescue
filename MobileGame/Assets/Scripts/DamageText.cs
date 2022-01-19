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

    // Start is called before the first frame update
    void Start()
    {
        invisColor = new Color(1, 1, 1, 0);
        sr = GetComponent<TextMeshPro>();
        StartCoroutine(FloatUp());
    }

    private IEnumerator FloatUp()
    {
        float timer = 0;
        Vector2 initpos = transform.position;
        initpos.x -= 0.2f;
        initpos.y += 0.15f;
        sr.sortingOrder = 1000;
        while (timer < time)
        {
            sr.color = Color.Lerp(Color.white, invisColor, colorFade.Evaluate(timer / time));
            transform.position = new Vector2(initpos.x, initpos.y + 0.4f*moveY.Evaluate(timer / time));
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
