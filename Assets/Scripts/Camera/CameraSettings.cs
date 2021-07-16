using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera Settings", menuName = "Custom/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    [HideInInspector] public Camera mainCam;

    [Header("")]
    public float scrollWheelScale;
    public bool invertScrollDirection;
    public float keyboardScrollScale;

    [Header("")]
    public float positionLerpSpeed;
    public float velocityDecay;

    [Header("")]
    public float defaultOrthographicSize;
    public float sizeLerpSpeed;
}
