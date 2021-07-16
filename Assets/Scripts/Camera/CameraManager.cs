using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private static CameraManager _instance;
    public static CameraManager Instance { get => _instance; }

    public CameraSettings settings;

    Transform topmostTransform;
    List<ICameraMover> movers;

    void Awake()
    {
        _instance = this;
        settings.mainCam = GetComponent<Camera>();
        topmostTransform = transform;
        movers = new List<ICameraMover>();

        StackScripts(typeof(CameraFocuser));
        StackScripts(typeof(CameraScroller));
        StackScripts(typeof(CameraPanner));
    }

    void StackScripts(System.Type type)
    {
        if (!typeof(ICameraMover).IsAssignableFrom(type))
        {
            Debug.LogError("Passed non-ICameraMover type to camera stacker");
        }

        GameObject newObj = new GameObject();
        newObj.name = type.FullName;
        ICameraMover cameraMover = (ICameraMover)newObj.AddComponent(type);
        cameraMover.SetSettings(settings);
        movers.Add(cameraMover);

        topmostTransform.parent = newObj.transform;
        topmostTransform = newObj.transform;
    }

    public void Focus(Vector3 point)
    {
        foreach (ICameraMover mover in movers)
        {
            mover.MoveToPoint(point);
        }
    }
}
