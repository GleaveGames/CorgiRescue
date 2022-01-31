using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            for(int i = 0; i < 3; i++)
            {
                float xOffset = Random.Range(-0.5f, 0.5f);
                float yOffset = Random.Range(-0.5f, 0.5f);
                Vector2 newPos = new Vector2(xOffset, yOffset) * magnitude;
                Vector3 newPos3 = new Vector3(Random.Range(-0.5f, 0.5f) * magnitude, Random.Range(-0.5f, 0.5f)*magnitude, 0);
                Debug.Log(newPos3);

                while (elapsedTime < (2*i*duration/6) + duration/6)
                {
                    transform.position = Vector3.Lerp(originalPos, originalPos + newPos3, (elapsedTime - (2 * i * duration / 6) / duration / 6));
                    elapsedTime += Time.deltaTime;
                    
                    yield return null;
                }

                while (elapsedTime < ((2*i*duration + duration/6)/6) + duration/6)
                {
                    transform.position = Vector3.Lerp(originalPos+ newPos3, originalPos, (elapsedTime - (2 * i * duration / 6) / duration / 6));
                    elapsedTime += Time.deltaTime;

                    yield return null;
                }
                transform.position = originalPos;

            }


            elapsedTime += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPos;
    }

}
