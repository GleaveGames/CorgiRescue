using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonJuice : MonoBehaviour
{
    [SerializeField]
    AnimationCurve JuiceIn;
    [SerializeField]
    AnimationCurve JuiceOut;
    [SerializeField]
    float JuiceTime;
    [SerializeField]
    float JuiceMultiplier;


    private void Start()
    {
        StartCoroutine(Juice());
        JuiceTime += Random.Range(-0.3f, 0.3f);
    }

    private IEnumerator Juice() 
    {
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        transform.GetChild(0).gameObject.SetActive(true);
        float counter = 0;
        Vector2 startpos = transform.position;
        while (counter < JuiceTime) 
        {
            transform.position = new Vector2(startpos.x + JuiceIn.Evaluate(counter / JuiceTime) * JuiceMultiplier, startpos.y);
            counter += Time.deltaTime;
            yield return null;
        }
        transform.position = startpos;
    }

    public IEnumerator JuiceOutButton() 
    {
        yield return new WaitForSeconds(Random.Range(0, 0.2f));
        float counter = 0;
        Vector2 startpos = transform.position;
        while (counter < JuiceTime)
        {
            transform.position = new Vector2(startpos.x + JuiceOut.Evaluate(counter / JuiceTime) * JuiceMultiplier, startpos.y);
            counter += Time.deltaTime;
            yield return null;
        }
        transform.position = startpos;
        gameObject.SetActive(false);
    }
}
