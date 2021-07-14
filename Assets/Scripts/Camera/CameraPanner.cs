using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour, ICameraMover
{
    Camera mainCam;
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

    public float lerpSpeed;
    public float slideSpeed = 0.1f;
    [Range(0, 10)] public float velocityDecay;

    void Awake()
    {
        mainCam = Camera.main;
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
            cameraVelocity -= cameraVelocity * Time.deltaTime * velocityDecay;
        }

        desiredCameraPosition -= cameraVelocity;
        transform.position = Vector3.Lerp(transform.position, desiredCameraPosition, lerpSpeed * Time.deltaTime);

        while (mousePositions.Count > 2 && mousePositions.Peek().time < Time.time - secondsOfPositionsToHold)
            mousePositions.Dequeue();
        mousePositions.Enqueue(new MouseAtTime { time = Time.time, position = Input.mousePosition });

        lastMousePosition = Input.mousePosition;
    }

    public void MoveToPoint(Vector3 point)
    {
        desiredCameraPosition = point;
    }

    Vector3 ScreenToWorldPointDifference(Vector3 screenPoint1, Vector3 screenPoint2)
    {
        return mainCam.ScreenToWorldPoint(screenPoint1) - mainCam.ScreenToWorldPoint(screenPoint2);
    }
}
