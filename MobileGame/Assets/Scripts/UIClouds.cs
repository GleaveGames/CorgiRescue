using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIClouds : MonoBehaviour
{
    List<Cloud> cloudsleft;
    List<Cloud> cloudsright;
    [SerializeField]
    AnimationCurve X;
    [SerializeField]
    AnimationCurve Y;
    
    void Start()
    {
        cloudsleft = new List<Cloud>();
        cloudsright = new List<Cloud>();
        for(int i = 0; i < transform.childCount/2; i++)
        {
            Cloud newcloud = new Cloud();
            newcloud.t = transform.GetChild(i);
            newcloud.initPos = transform.GetChild(i).position;
            cloudsleft.Add(newcloud);
        }
        for(int i = transform.childCount/2; i < transform.childCount; i++)
        {
            Cloud newcloud = new Cloud();
            newcloud.t = transform.GetChild(i);
            newcloud.initPos = transform.GetChild(i).position;
            cloudsright.Add(newcloud);
        }
        StartCoroutine(Leave());
    }

    public IEnumerator Enter()
    {
        foreach (Cloud c in cloudsleft)
        {
            StartCoroutine(cloudMove(c.t, c.t.position, c.initPos, Random.Range(1f, 1.5f)));
        }
        foreach (Cloud c in cloudsright)
        {
            StartCoroutine(cloudMove(c.t, c.t.position, c.initPos, Random.Range(1f, 1.5f)));
        }
        yield return null;
    }

    public IEnumerator Leave()
    {
        yield return new WaitForSeconds(1);
        foreach(Cloud c in cloudsleft)
        {
            Vector2 end = c.initPos;
            end.x -= Random.Range(2500,3500);
            end.y -= Random.Range(-300,300);
            StartCoroutine(cloudMove(c.t, c.initPos, end, Random.Range(1.5f,2f)));
        }
        foreach(Cloud c in cloudsright)
        {
            Vector2 end = c.initPos;
            end.x += Random.Range(2500,3500);
            end.y += Random.Range(-300,300);
            StartCoroutine(cloudMove(c.t, c.initPos, end, Random.Range(1.5f, 2f)));
        }
        yield return null;
    }

    IEnumerator cloudMove(Transform c, Vector2 start, Vector2 end, float moveTime)
    {
        yield return new WaitForSeconds(Random.Range(0, 0.4f));
        float timer = 0;
        while (timer < moveTime)
        {
            Vector2 newPos = new Vector2(Mathf.Lerp(start.x, end.x, X.Evaluate(timer / moveTime)), Mathf.Lerp(start.y, end.y, timer / moveTime) + 500 * Y.Evaluate(timer / moveTime));
            c.position = newPos;
            timer += Time.deltaTime;
            yield return null;
        }
    }

}

public class Cloud
{
    public Transform t;
    public Vector2 initPos;
}


