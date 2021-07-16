using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour, ICameraMover
{
    CameraSettings settings;

    Vector3 lastMousePosition;
    Vector3 cameraVelocity;
    Vector3 desiredCameraPosition;

    struct MouseAtTime
    {
        public float time;
        public Vector3 position;
    }
    Queue<MouseAtTime> mousePositions;
    float secondsOfPositionsToHold = 0.05f;

    void Awake()
    {
        mousePositions = new Queue<MouseAtTime>();
    }

    void LateUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            cameraVelocity = ScreenToWorldPointDifference(Input.mousePosition, lastMousePosition);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            cameraVelocity = ScreenToWorldPointDifference(Input.mousePosition, mousePositions.Peek().position);
            cameraVelocity /= mousePositions.Count;
        }
        else
        {
            cameraVelocity -= cameraVelocity * Time.deltaTime * settings.velocityDecay;
        }

        desiredCameraPosition -= cameraVelocity;
        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, settings.positionLerpSpeed * Time.deltaTime);

        while (mousePositions.Count > 2 && mousePositions.Peek().time < Time.time - secondsOfPositionsToHold)
            mousePositions.Dequeue();
        mousePositions.Enqueue(new MouseAtTime { time = Time.time, position = Input.mousePosition });

        lastMousePosition = Input.mousePosition;
    }

    Vector3 ScreenToWorldPointDifference(Vector3 screenPoint1, Vector3 screenPoint2)
    {
        return settings.mainCam.ScreenToWorldPoint(screenPoint1) - settings.mainCam.ScreenToWorldPoint(screenPoint2);
    }

    public void MoveToPoint(Vector3 point)
    {
        cameraVelocity = Vector3.zero;
        desiredCameraPosition = transform.position;
    }

    public void SetSettings(CameraSettings settings)
    {
        this.settings = settings;
    }
}
