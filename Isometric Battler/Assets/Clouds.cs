using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clouds : MonoBehaviour
{
    [SerializeField]
    List<Sprite> clouds;
    [SerializeField]
    int numClouds;
    [SerializeField]
    float cloudSpeed;
    [SerializeField]
    float spawnInterval;
    [SerializeField]
    List<Transform> CloudSpawns;
    [SerializeField]
    Color full;
    [SerializeField]
    Color empty;
    [SerializeField]
    float ColorChangeSpeed = 1;

    private void Start()
    {
        StartCoroutine(SpawnClouds());
    }

    private IEnumerator SpawnClouds()
    {
        float counter = 0;
        for (int i = 0; i < numClouds; i++) 
        {
            while (counter < spawnInterval) 
            {
                counter += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(CreateCloud());
            counter = 0;
        }
    }

    private IEnumerator CreateCloud() 
    {
        float counter = 0;
        float cloudLifetime = Random.Range(8, 18f);
        float speed = cloudSpeed + Random.Range(0, 1f);
        GameObject cloud = Instantiate(new GameObject(), CloudSpawns[Random.Range(0, CloudSpawns.Count - 1)].position, Quaternion.identity);
        cloud.AddComponent<SpriteRenderer>().sprite = clouds[Random.Range(0, clouds.Count - 1)];
        cloud.transform.parent = transform;
        cloud.GetComponent<SpriteRenderer>().sortingOrder = 10;
        Vector2 pos = cloud.transform.position;
        while(counter < cloudLifetime) 
        {
            if (counter < ColorChangeSpeed)
            {
                cloud.GetComponent<SpriteRenderer>().color = Color.Lerp(empty, full, counter / ColorChangeSpeed);
            }
            else if (counter > cloudLifetime - ColorChangeSpeed)
            {
                cloud.GetComponent<SpriteRenderer>().color = Color.Lerp(full, empty, (counter - (cloudLifetime - ColorChangeSpeed))/ ColorChangeSpeed);
            }
            pos.x += speed * Time.deltaTime;
            cloud.transform.position = pos;
            counter += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(CreateCloud());
        Destroy(cloud);
    }    
}
