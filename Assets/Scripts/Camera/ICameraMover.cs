using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICameraMover
{
    void MoveToPoint(Vector3 point);
    void SetSettings(CameraSettings settings);
}
