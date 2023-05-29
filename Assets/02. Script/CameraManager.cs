using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float minCameraX = -10.0f;
    public float maxCameraX = 10.0f;
    public float minCameraY = -10.0f;
    public float maxCameraY = 10.0f;

    void LateUpdate()
    {
        Vector3 cameraPosition = transform.position;
        cameraPosition.x = Mathf.Clamp(cameraPosition.x, minCameraX, maxCameraX);
        cameraPosition.y = Mathf.Clamp(cameraPosition.y, minCameraY, maxCameraY);

        transform.position = cameraPosition;
    }
}
