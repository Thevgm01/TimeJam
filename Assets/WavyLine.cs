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

    public float scale = 10f;
    public AnimationCurve noiseScaleOverLength;

    Vector3[] basePositions;

    LineRenderer lr;
    float factor = 0.5f;
    float seed;

    // Start is called before the first frame update
    void Awake()
    {
        lr = GetComponent<LineRenderer>();
        seed = Random.Range(0f, 100000f);
    }

    // Update is called once per frame
    void Update()
    {
        SetBasePositions();
        ApplyNoise();
    }

    void SetBasePositions()
    {
        float length = (destination.position - transform.position).magnitude;
        if (length != lastLength)
        {
            int numPositions = Mathf.RoundToInt(length / resolutionPerUnit);
            basePositions = new Vector3[numPositions];
            basePositions[0] = transform.position;
            basePositions[numPositions - 1] = destination.position;

            Vector3 diff = destination.position - transform.position;
            Vector3 resolutionDiff = diff.normalized * resolutionPerUnit;
            for (int i = 1; i < numPositions - 1; ++i)
            {
                basePositions[i] = basePositions[i - 1] + resolutionDiff;
            }

            lr.positionCount = numPositions;
            lr.SetPositions(basePositions);

            lastLength = length;
        }
    }

    void ApplyNoise()
    {
        for (int i = 1; i < basePositions.Length - 1; i++)
        {
            float frac = (float)i / (basePositions.Length - 1);
            float curveScale = noiseScaleOverLength.Evaluate(frac);

            float curScale = scale;
            float time = Time.time;

            Vector3 point = basePositions[i];
            float length = point.magnitude / scale;

            Vector3 offset = new Vector3();
            offset.x += Mathf.PerlinNoise(length - time - seed, seed);
            offset.y += Mathf.PerlinNoise(seed, length - time - seed);

            offset = offset * 2 - new Vector3(1, 1, 0);
            offset = offset * curScale * curveScale;
            lr.SetPosition(i, point + offset);
        }
    }
}
