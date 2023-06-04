using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CameraShake : MonoBehaviour
{
    public float shakeAmount;
    public float shakeDuration;
    private Vector3 shakeOffset;

    public void Shake()
    {
        StartCoroutine(StartShake());
    }

    private IEnumerator StartShake()
    {
        float startTime = Time.time;
        while (Time.time < startTime + shakeDuration)
        {
            shakeOffset = Random.insideUnitSphere * shakeAmount;
            shakeOffset.z = 0; 
            yield return null;
        }
        shakeOffset = Vector3.zero;
    }

    public Vector3 GetShakeOffset()
    {
        return shakeOffset;
    }
}