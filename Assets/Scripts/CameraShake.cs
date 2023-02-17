using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public IEnumerator CameraShakeController(float shakeDuration, float shakeMagnitude, float magnitudeFade, float waitBeforeShake = 0) {
        Vector2 originalPosition = transform.localPosition;
        float elapsedTime = 0;

        yield return new WaitForSeconds(waitBeforeShake);

        while (elapsedTime < shakeDuration) {
            float xAxisShift = Random.Range(-1f, 1f) * shakeMagnitude;
            float yAxisShift = Random.Range(-1f, 1f) * shakeMagnitude;
            shakeMagnitude *= magnitudeFade;

            transform.localPosition = new Vector2(xAxisShift, yAxisShift);
            
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}
