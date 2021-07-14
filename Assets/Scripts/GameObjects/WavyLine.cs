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
    [Range(0, 0.02f)] public float scrollTimeDelay = 0.01f;
    [Range(-3, 3)] public float textureSpeed = 1f;
    public AnimationCurve noiseScaleOverLength;
    float noiseTimeTracker = 0;

    Vector3[] basePositions;
    Vector3[] noiseOffsets;

    LineRenderer lr;
    Material mat;

    void Awake()
    {
        noise = new Noise();

        lr = GetComponent<LineRenderer>();
        lr.sharedMaterial.mainTextureOffset = Vector2.zero;
    }

    void Start()
    {
        SetBasePositions();
    }

    // Update is called once per frame
    void Update()
    {
        SetBasePositions();
        noiseTimeTracker += Time.deltaTime * speed;
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
            basePositions[0] = Vector3.zero;
            basePositions[numPositions - 1] = destination.position - transform.position;

            Vector3 diff = destination.position - transform.position;
            Vector3 resolutionDiff = diff.normalized * resolutionPerUnit;
            for (int i = 1; i < numPositions - 1; ++i)
            {
                basePositions[i] = basePositions[i - 1] + resolutionDiff;
            }

            noiseOffsets = new Vector3[numPositions];
            SetFirstNOffsets(numPositions);

            lr.positionCount = numPositions;
            SetLinePoints();

            lastLength = length;
        }
    }
    float timeTracker = 0;
    void ApplyNoise()
    {
        if (!lr.isVisible)
            return;

        if (noise == null)
            noise = new Noise();

        timeTracker += Time.deltaTime;
        int positionsAdvanced = 0;
        if (scrollTimeDelay > 0)
        {
            while (timeTracker >= scrollTimeDelay)
            {
                ++positionsAdvanced;
                timeTracker -= scrollTimeDelay;
            }
            if (positionsAdvanced > basePositions.Length)
            {
                positionsAdvanced = basePositions.Length;
            }
        }
        else
        {
            positionsAdvanced = basePositions.Length;
        }

        if (positionsAdvanced > 0)
        {
            MoveNOffsets(positionsAdvanced);
            SetFirstNOffsets(positionsAdvanced);
            SetLinePoints();
        }
    }

    void MoveNOffsets(int n)
    {
        for (int i = noiseOffsets.Length - 1; i >= n; --i)
        {
            noiseOffsets[i] = noiseOffsets[i - n];
        }
    }

    void SetFirstNOffsets(int n)
    {
        for (int i = 0; i < n; ++i)
        {
            Vector3 point = basePositions[i];
            Vector3 offset = new Vector3();

            float length = point.magnitude * frequency;

            offset.x += noise.Evaluate(length + noiseTimeTracker, 0);
            offset.y += noise.Evaluate(0, length + noiseTimeTracker);

            noiseOffsets[i] = offset;
        }
    }

    void SetLinePoints()
    {
        for (int i = 0; i < basePositions.Length; ++i)
        {
            float frac = (float)i / (basePositions.Length - 1);
            float curveScale = noiseScaleOverLength.Evaluate(frac);
            Vector3 pos = basePositions[i] + noiseOffsets[i] * amplitude * curveScale;
            lr.SetPosition(i, pos);
        }

    }

    void ScrollTexture()
    {
        lr.sharedMaterial.mainTextureOffset += Vector2.right * textureSpeed * Time.deltaTime;
    }
}
