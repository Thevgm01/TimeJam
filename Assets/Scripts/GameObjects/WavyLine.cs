using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class WavyLine : MonoBehaviour
{
    Noise noise;

    public Transform origin;
    public Transform destination;
    [Range(0.0f, 3.0f)] public float resolution;
    float lastLength = 0;

    [Header("Noise")]
    [Range(0, 30)] public float amplitude = 10f;
    [Range(0, 1.0f)] public float frequency = 1f;
    [Range(-3, 3)] public float speed = 1f;
    [Range(-3, 3)] public float textureSpeed = 1f;
    public AnimationCurve noiseScaleOverLength;
    float noiseTimeTracker = 0;

    Vector3[] basePositions;
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
        float length = Vector3.Distance(origin.position, destination.position);
        if (length != lastLength)
        {
            SetBasePositions(length);
        }
        ApplyNoise();
        ScrollTexture(length);
        lastLength = length;
    }

    void SetBasePositions(float length)
    {
        int numPositions = Mathf.CeilToInt(length * resolution);

        basePositions = new Vector3[numPositions];
        basePositions[0] = origin.position;
        basePositions[numPositions - 1] = destination.position;

        curveScales = new float[numPositions];
        curveScales[0] = 0.0f;
        curveScales[numPositions - 1] = 0.0f;

        lr.positionCount = numPositions;
        lr.SetPosition(0, origin.position);
        lr.SetPosition(numPositions - 1, destination.position);

        for (int i = 1; i < numPositions - 1; ++i)
        {
            float frac = (float)i / numPositions;
            basePositions[i] = Vector3.Lerp(origin.position, destination.position, frac);
            curveScales[i] = noiseScaleOverLength.Evaluate(frac);
        }
    }

    void ApplyNoise()
    {
        if (!lr.isVisible)
            return;

        noiseTimeTracker += Time.deltaTime * speed;
        for (int i = 1; i < basePositions.Length - 1; i++)
        {
            float frac = (float)i / basePositions.Length;

            Vector3 offset = new Vector3();
            offset.x += noise.Evaluate(frac * frequency + noiseTimeTracker, 0);
            offset.y += noise.Evaluate(0, frac * frequency + noiseTimeTracker);
            offset = offset * amplitude * curveScales[i];

            lr.SetPosition(i, basePositions[i] + offset);
        }
    }

    void ScrollTexture(float length)
    {
        lr.material.mainTextureOffset += Vector2.right * (textureSpeed * Time.deltaTime + (lastLength - length) * lr.material.mainTextureScale.x / 2.0f);
    }
}
