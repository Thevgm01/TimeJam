using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[ExecuteAlways]
public class WavyLine : MonoBehaviour
{
    Noise noise;

    public Transform destination;
    [Range(0, 1)] public float resolutionPerUnit;
    float lastLength = 0;

    [Header("Noise")]
    [Range(0, 30)] public float amplitude = 10f;
    [Range(0, 0.2f)] public float frequency = 1f;
    [Range(-3, 3)] public float speed = 1f;
    [Range(-3, 3)] public float textureSpeed = 1f;
    public AnimationCurve noiseScaleOverLength;
    float noiseTimeTracker = 0;

    Vector3[] basePositions;
    float[] lengths;
    float[] curveScales;

    LineRenderer lr;
    Material mat;

    // Start is called before the first frame update
    void Awake()
    {
        noise = new Noise();

        lr = GetComponent<LineRenderer>();
        lr.sharedMaterial.mainTextureOffset = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        SetBasePositions();
        ApplyNoise();
        ScrollTexture();
    }

    void SetBasePositions()
    {
        float length = (destination.position - transform.position).magnitude;
        if (length != lastLength)
        {
            int numPositions = Mathf.RoundToInt(length / resolutionPerUnit);
            basePositions = new Vector3[numPositions];
            lengths = new float[numPositions];
            curveScales = new float[numPositions];

            basePositions[0] = Vector3.zero;
            basePositions[numPositions - 1] = destination.position - transform.position;
            Vector3 diff = destination.position - transform.position;
            Vector3 resolutionDiff = diff / numPositions;

            lr.positionCount = numPositions;
            lr.SetPosition(0, basePositions[0]);
            lr.SetPosition(numPositions - 1, basePositions[numPositions - 1]);

            for (int i = 1; i < numPositions - 1; ++i)
            {
                basePositions[i] = basePositions[i - 1] + resolutionDiff;

                lengths[i] = basePositions[i].magnitude;

                float frac = (float)i / (basePositions.Length - 1);
                float curveScale = noiseScaleOverLength.Evaluate(frac);
                curveScales[i] = curveScale;
            }

            lastLength = length;
        }
    }

    void ApplyNoise()
    {
        if (!lr.isVisible)
            return;

        if (noise == null)
            noise = new Noise();

        noiseTimeTracker += Time.deltaTime * speed;
        for (int i = 1; i < basePositions.Length - 1; i++)
        {
            Vector3 point = basePositions[i];
            Vector3 offset = new Vector3();

            float length = lengths[i] * frequency;

            offset.x += noise.Evaluate(length + noiseTimeTracker, 0);
            offset.y += noise.Evaluate(0, length + noiseTimeTracker);

            offset = offset * amplitude * curveScales[i];
            lr.SetPosition(i, point + offset);
        }
    }

    void ScrollTexture()
    {
        lr.sharedMaterial.mainTextureOffset += Vector2.right * textureSpeed * Time.deltaTime;
    }
}
