using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour
{
    Camera mainCam;
    public void ShakeCam(float time, float magnitude)
    {
        mainCam = Camera.main;
        StartCoroutine(Shake(time, magnitude));
    }
    IEnumerator Shake(float time, float magnitude)
    {
        Vector3 startPosition = new Vector3(0, 0, -10);
        Vector3 startRotation = Vector3.zero;

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            mainCam.transform.localPosition = startPosition + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f) * magnitude);
            mainCam.transform.localEulerAngles = startRotation + new Vector3(0, 0, Random.Range(-1f, 1f) * magnitude);
            yield return null;
        }

        mainCam.transform.localPosition = startPosition;
        mainCam.transform.localEulerAngles = startRotation;
    }
}
