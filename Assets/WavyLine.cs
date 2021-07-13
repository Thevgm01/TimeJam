using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class WavyLine : MonoBehaviour
{
    Noise noise;

    public Transform destination;
    public float resolutionPerUnit;
    float lastLength = 0;

    [Range(0, 10)] public float amplitude = 10f;
    [Range(0, 1)] public float frequency = 1f;
    public AnimationCurve noiseScaleOverLength;

    Vector3[] basePositions;

    LineRenderer lr;
    float factor = 0.5f;
    float seed;

    // Start is called before the first frame update
    void Awake()
    {
        noise = new Noise();

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
            basePositions[0] = Vector3.zero;
            basePositions[numPositions - 1] = destination.position - transform.position;

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
        if (!lr.isVisible)
            return;

        if (noise == null)
            noise = new Noise();

        for (int i = 0; i < basePositions.Length; i++)
        {
            float frac = (float)i / (basePositions.Length - 1);
            float curveScale = noiseScaleOverLength.Evaluate(frac);

            float time = Time.time;

            Vector3 point = basePositions[i];
            Vector3 offset = new Vector3();

            float length = point.magnitude * frequency;

            offset.x += noise.Evaluate(new Vector3(length - time - seed, seed, 0));
            offset.y += noise.Evaluate(new Vector3(seed, length - time - seed, 0));

            offset = offset * 2 - new Vector3(1, 1, 0);
            offset = offset * amplitude * curveScale;
            lr.SetPosition(i, transform.position + point + offset);
        }
    }
}
