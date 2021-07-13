using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class WavyLine : MonoBehaviour
{
    public Transform destination;
    public float resolutionPerUnit;
    float lastLength = 0;

    Vector3[] basePositions;

    LineRenderer lr;
    float scale = 10f;
    float offset = 0f;
    float factor = 0.5f;

    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        SetPositions();
        ApplyNoise();
    }

    void SetPositions()
    {
        float length = (destination.position - transform.position).magnitude;
        if (length != lastLength)
        {
            int numPositions = Mathf.RoundToInt(length / resolutionPerUnit);
            basePositions = new Vector3[numPositions];
            for (int i = 0; i < numPositions; ++i)
            {
                float frac = (float)i / (numPositions - 1);
                basePositions[i] = Vector3.Lerp(transform.position, destination.position, frac);
            }
            lr.positionCount = numPositions;
            lr.SetPositions(basePositions);
            lastLength = length;
        }
    }

    void ApplyNoise()
    {
        float curScale = scale;
        for (int i = 1; i < basePositions.Length - 1; i++)
        {
            float frac = (float)i / (basePositions.Length - 1);
            Vector3 point = basePositions[i];
            //for (int j = 0; j < 3; ++j)
            //{

            //}
            point.x += Mathf.PerlinNoise(point.x + Time.time, point.y);
            point.y += Mathf.PerlinNoise(point.x, point.y + Time.time);
            //scale *= factor;
            lr.SetPosition(i, point);
        }
    }
}
