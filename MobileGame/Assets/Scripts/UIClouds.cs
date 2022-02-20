using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIClouds : MonoBehaviour
{
    List<Cloud> cloudsleft;
    List<Cloud> cloudsright;
    [SerializeField]
    AnimationCurve X;
    [SerializeField]
    AnimationCurve Y;
    [SerializeField]
    List<Sprite> confettiSprites;


    
    void Start()
    {
        cloudsleft = new List<Cloud>();
        cloudsright = new List<Cloud>();
        for(int i = 0; i < transform.childCount/2; i++)
        {
            Cloud newcloud = new Cloud();
            newcloud.t = transform.GetChild(i);
            newcloud.initPos = transform.GetChild(i).position - transform.parent.position;
            cloudsleft.Add(newcloud);
        }
        for(int i = transform.childCount/2; i < transform.childCount; i++)
        {
            Cloud newcloud = new Cloud();
            newcloud.t = transform.GetChild(i);
            newcloud.initPos = transform.GetChild(i).position - transform.parent.position;
            cloudsright.Add(newcloud);
        }
        if(SceneManager.GetActiveScene().buildIndex != 0) StartCoroutine(Leave());
    }

    public IEnumerator Enter()
    {
        foreach (Cloud c in cloudsleft)
        {
            StartCoroutine(cloudMove(c.t, c.t.position, c.initPos + new Vector2(transform.parent.position.x, transform.parent.position.y), Random.Range(1f, 1.5f)));
        }
        foreach (Cloud c in cloudsright)
        {
            StartCoroutine(cloudMove(c.t, c.t.position, c.initPos + new Vector2(transform.parent.position.x, transform.parent.position.y), Random.Range(1f, 1.5f)));
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
            StartCoroutine(cloudMove(c.t, c.initPos + new Vector2(transform.parent.position.x, transform.parent.position.y), end, Random.Range(1.5f,2f)));
        }
        foreach(Cloud c in cloudsright)
        {
            Vector2 end = c.initPos;
            end.x += Random.Range(2500,3500);
            end.y += Random.Range(-300,300);
            StartCoroutine(cloudMove(c.t, c.initPos + new Vector2(transform.parent.position.x, transform.parent.position.y), end, Random.Range(1.5f, 2f)));
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

    public IEnumerator Confetti(int length)
    {
        float timer = 0;
        int confettiCount = 100;
        List<Transform> confettis = new List<Transform>();
        List<float> confettiWeights = new List<float>();
        List<Vector2> confettiDisplacements = new List<Vector2>();
        float minX = transform.parent.GetComponent<RectTransform>().position.x + transform.parent.GetComponent<RectTransform>().rect.xMin;
        float maxY = transform.parent.GetComponent<RectTransform>().position.y + transform.parent.GetComponent<RectTransform>().rect.yMax;
        float z = transform.parent.GetComponent<RectTransform>().position.z;
        float speed = 14f;
        float depth = 0.002f;

        Vector3 topLeft = new Vector3(minX, maxY, z);
        for (int i = 0; i < confettiCount; i++)
        {
            GameObject confetti = new GameObject();
            confetti.AddComponent<Image>();
            confetti.transform.parent = transform;
            confettis.Add(confetti.transform);
            confetti.GetComponent<Image>().sprite = confettiSprites[Random.Range(0, confettiSprites.Count)];
            Vector3 spawnPos = topLeft;
            spawnPos.x += Random.Range(0, 2* transform.parent.GetComponent<RectTransform>().rect.xMax);
            spawnPos.y += Random.Range(0, 400);
            confetti.transform.position = spawnPos;
            confetti.transform.localScale = new Vector3(0.2f, 0.2f, 1);
            confettiWeights.Add(1);
            confettiDisplacements.Add(new Vector2(Random.Range(-1000, 1000), Random.Range(-1000, 1000)));

        }
        while (timer <= length)
        {
            for(int i = 0; i < confettis.Count; i++)
            {
                confettis[i].position += Vector3.right * (Mathf.PerlinNoise((confettis[i].position.x + confettiDisplacements[i].x)*depth, (confettis[i].position.y + confettiDisplacements[i].y) * depth) - 0.45f) * speed * confettiWeights[i];
                confettis[i].position += Vector3.down * speed * (0.3f-(Mathf.Abs(Mathf.PerlinNoise((confettis[i].position.x + confettiDisplacements[i].x) * depth, (confettis[i].position.y + confettiDisplacements[i].y) * depth)-0.45f)));
            }
            timer += Time.deltaTime;
            yield return null;
        }

        foreach(Transform t in confettis)
        {
            Destroy(t.gameObject);
        }
    }

    public void ConfettiTest()
    {
        StartCoroutine(Confetti(100));
    }
    
}



public class Cloud
{
    public Transform t;
    public Vector2 initPos;
}


