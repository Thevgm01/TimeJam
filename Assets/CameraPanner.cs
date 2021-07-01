using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPanner : MonoBehaviour
{
    Vector3 velocity, desiredPosition;
    public float lerpSpeed;
    [Range(0, 1)] public float flingDecayStrength;

    void Awake()
    {
        velocity = Vector3.zero;
        desiredPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            velocity.x = -Input.GetAxis("Mouse X");
            velocity.y = -Input.GetAxis("Mouse Y");
        }
        else if (velocity.sqrMagnitude > 0)
        {
            velocity *= 1 - flingDecayStrength;
        }

        desiredPosition += velocity;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, lerpSpeed * Time.deltaTime);
    }
}
