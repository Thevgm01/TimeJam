using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroller : MonoBehaviour
{
    public float scrollWheelScale;
    public bool invertScrollDirection;
    public float keyboardScale;
    public float lerpSpeed;

    Camera mainCam;
    float desiredSize;

    // Start is called before the first frame update
    void Awake()
    {
        mainCam = Camera.main;
        desiredSize = mainCam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float scrollAmount = 0f;
        scrollAmount += Input.mouseScrollDelta.y * scrollWheelScale * (invertScrollDirection ? -1 : 1);
        scrollAmount -= Input.GetKey(KeyCode.R) ? keyboardScale : 0;
        scrollAmount += Input.GetKey(KeyCode.F) ? keyboardScale : 0;

        if (scrollAmount != 0)
        {
            desiredSize += scrollAmount * Time.deltaTime;
        }

        mainCam.orthographicSize = Mathf.Lerp(mainCam.orthographicSize, desiredSize, lerpSpeed * Time.deltaTime);
    }
}
