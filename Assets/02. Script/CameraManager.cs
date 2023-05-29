using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
    public float minCameraX = -10.0f;
    public float maxCameraX = 10.0f;
    public float minCameraY = -10.0f;
    public float maxCameraY = 10.0f;

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 targetPosition = target.position;
            targetPosition.x = Mathf.Clamp(targetPosition.x, minCameraX, maxCameraX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minCameraY, maxCameraY);

            transform.position = new Vector3(targetPosition.x, targetPosition.y, transform.position.z);
        }
    }
}
