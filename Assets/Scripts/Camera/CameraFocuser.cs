using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocuser : MonoBehaviour
{
    private static CameraFocuser _instance;
    public static CameraFocuser Instance { get => _instance; }

    public float lerpSpeed;
    Vector3 desiredLocalPosition;

    void Awake()
    {
        _instance = this;
    }

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocalPosition, lerpSpeed * Time.deltaTime);
    }

    public void Focus(Vector3 pos)
    {
        desiredLocalPosition = pos - transform.parent.position;
    }
}
