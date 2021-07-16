using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour, ICameraMover
{
    CameraSettings settings;

    float desiredSize;
    Vector3 mousePositionAtScroll;
    Vector3 desiredCameraPosition;

    void LateUpdate()
    {
        float scrollAmount = 0f;
        scrollAmount += Input.mouseScrollDelta.y * settings.scrollWheelScale * (settings.invertScrollDirection ? -1 : 1);
        scrollAmount -= Input.GetKey(KeyCode.R) ? settings.keyboardScrollScale : 0;
        scrollAmount += Input.GetKey(KeyCode.F) ? settings.keyboardScrollScale : 0;

        if (scrollAmount != 0)
        {
            desiredSize += scrollAmount * Time.deltaTime;
            mousePositionAtScroll = Input.mousePosition;
        }

        Vector3 originalMouseWorld = settings.mainCam.ScreenToWorldPoint(mousePositionAtScroll);
        settings.mainCam.orthographicSize = Mathf.Lerp(settings.mainCam.orthographicSize, desiredSize, settings.sizeLerpSpeed * Time.deltaTime);
        Vector3 newMouseWorld = settings.mainCam.ScreenToWorldPoint(mousePositionAtScroll);

        Vector3 diff = originalMouseWorld - newMouseWorld;
        desiredCameraPosition = transform.position + diff;
        desiredCameraPosition.z = 0;
        transform.position = desiredCameraPosition;
    }

    public void MoveToPoint(Vector3 point)
    {
        desiredSize = settings.defaultOrthographicSize;
        mousePositionAtScroll = settings.mainCam.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0));
    }

    public void SetSettings(CameraSettings settings)
    {
        this.settings = settings;
        desiredSize = settings.defaultOrthographicSize;
    }
}
