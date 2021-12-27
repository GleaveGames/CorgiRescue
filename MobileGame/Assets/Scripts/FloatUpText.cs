using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatUpText : MonoBehaviour
{
    SpriteRenderer sr;
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
        sr = GetComponent<SpriteRenderer>();
        StartCoroutine(FloatUp());
    }

    private IEnumerator FloatUp()
    {
        float timer = 0;
        Vector2 initpos = transform.position;
        while (timer < time)
        {
            sr.color = Color.Lerp(Color.white, invisColor, colorFade.Evaluate(timer / time));
            transform.position = new Vector2(initpos.x, initpos.y + moveY.Evaluate(timer / time));
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(gameObject);
    }
}
