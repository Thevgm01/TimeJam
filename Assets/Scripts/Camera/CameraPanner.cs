using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour, ICameraMover
{
    Camera mainCam;
    Vector3 lastMousePosition;
    Vector3 cameraVelocity;
    Vector3 desiredCameraPosition;

    public float lerpSpeed;
    [Range(0, 1)] public float velocityDecay;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            cameraVelocity = mainCam.ScreenToWorldPoint(Input.mousePosition) - mainCam.ScreenToWorldPoint(lastMousePosition);
        }

        desiredCameraPosition -= cameraVelocity;
        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, lerpSpeed * Time.deltaTime);

        lastMousePosition = Input.mousePosition;
    }

    void FixedUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
            cameraVelocity *= 1f - velocityDecay;
        }
    }

    public void MoveToPoint(Vector3 point)
    {
        desiredCameraPosition = point;
    }
}
