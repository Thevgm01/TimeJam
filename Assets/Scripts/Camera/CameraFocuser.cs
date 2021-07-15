using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocuser : MonoBehaviour
{
    public float lerpSpeed;
    Vector3 desiredLocalPosition;

    void Update()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, desiredLocalPosition, lerpSpeed * Time.deltaTime);
    }

    public void Focus(Vector3 pos)
    {
        desiredLocalPosition = pos - transform.parent.position;
    }
}
