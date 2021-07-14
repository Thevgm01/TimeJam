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

    [Header("Noise")]
    [Range(0, 30)] public float amplitude = 10f;
    [Range(0, 0.2f)] public float frequency = 1f;
    [Range(-3, 3)] public float speed = 1f;
    [Range(-3, 3)] public float textureSpeed = 1f;
    public AnimationCurve noiseScaleOverLength;
    float noiseTimeTracker = 0;

    Vector3[] basePositions;

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

        noiseTimeTracker += Time.deltaTime * speed;
        for (int i = 0; i < basePositions.Length; i++)
        {
            float frac = (float)i / (basePositions.Length - 1);
            float curveScale = noiseScaleOverLength.Evaluate(frac);

            Vector3 point = basePositions[i];
            Vector3 offset = new Vector3();

            float length = point.magnitude * frequency;

            offset.x += noise.Evaluate(length + noiseTimeTracker, 0);
            offset.y += noise.Evaluate(0, length + noiseTimeTracker);

            //offset -= new Vector3(0.5f, 0.5f, 0);
            offset = offset * amplitude * curveScale;
            lr.SetPosition(i, point + offset);
        }
    }

    void ScrollTexture()
    {
        lr.sharedMaterial.mainTextureOffset += Vector2.right * textureSpeed * Time.deltaTime;
    }
}
