using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator Shake()
    {
        float duration = 0.02f;
        float magnitude = 0.2f;
        Vector3 originalPos = transform.position;
        float elapsedTime = 0f;
        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 3; i++)
        {
            elapsedTime = 0f;
            float xOffset = Random.Range(-0.5f, 0.5f);
            float yOffset = Random.Range(-0.5f, 0.5f);
            Vector2 newPos = new Vector2(xOffset, yOffset) * magnitude;
            Vector3 newPos3 = new Vector3(Random.Range(-0.5f, 0.5f) * magnitude, Random.Range(-0.5f, 0.5f)*magnitude, 0);
            while (elapsedTime <= duration)
            {
                transform.position = Vector3.Lerp(originalPos, originalPos + newPos3, elapsedTime/(duration));
                elapsedTime += Time.deltaTime;
                    
                yield return null;
            }
            elapsedTime = 0f;

            while (elapsedTime <= duration)
            {
                transform.position = Vector3.Lerp(originalPos+ newPos3, originalPos, (elapsedTime/duration));
                elapsedTime += Time.deltaTime;

                yield return null;
            }
            transform.position = originalPos;
        }

        transform.position = originalPos;
    }

}
