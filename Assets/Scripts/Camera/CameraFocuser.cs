using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocuser : MonoBehaviour, ICameraMover
{
    CameraSettings settings;

    Vector3 desiredLocalPosition;

    void LateUpdate()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocalPosition, settings.positionLerpSpeed * Time.deltaTime);
    }

    public void MoveToPoint(Vector3 pos)
    {
        desiredLocalPosition = pos - transform.parent.position;
    }

    public void SetSettings(CameraSettings settings)
    {
        this.settings = settings;
    }
}
