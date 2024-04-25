using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePerimeter : MonoBehaviour
{
    public int numPoints = 100;
    public float radius = 50f;
    public float noiseScale = 5f;
    public int singels;
    public float streetWidth = 2f;
    [SerializeField] float radiusDecrement;
    [SerializeField] float radiusDecrementNoise;
    [SerializeField] float noiseDecrement;
    [SerializeField] bool reverseRingOrder;
    [SerializeField] bool randomMiddle;
    [SerializeField] float middleNoise;
    Vector3[] points;
    List<float> ringRadiusList;
    [SerializeField] List<GameObject> ringsAndPerimeter;
    
    void Start()
    {
        ringRadiusList = new List<float>();
        ringsAndPerimeter = new List<GameObject>();
        ringsAndPerimeter.Add(gameObject);
        GenerateCircularPerimeter();
        AddRings(singels, radiusDecrement, noiseDecrement, reverseRingOrder);
        GenerateStreets();
    }

    #region Streets
    void GenerateStreets()
    {
        // Connect each point on the perimeter to the corresponding point on each ring
        for (int i = 0; i < numPoints; i++)
        {
            if(Random.Range(0,2) < 1)
            {
                for (int ringIndex = 1; ringIndex <= singels; ringIndex++)
                {
                        Vector3 perimeterPoint = GetRingPoint(i, ringIndex - 1);
                        Vector3 ringPoint = GetRingPoint(i, ringIndex);

                        // Create a street segment between the points
                        GameObject streetSegment = new GameObject("StreetSegment" + i + "_Ring" + ringIndex);
                        streetSegment.transform.parent = transform;

                        LineRenderer streetRenderer = streetSegment.AddComponent<LineRenderer>();
                        streetRenderer.positionCount = 2;
                        streetRenderer.SetPositions(new Vector3[] { perimeterPoint, ringPoint });
                        streetRenderer.startWidth = streetWidth;
                        streetRenderer.endWidth = streetWidth;
                }
            }
        }
    }


    Vector3 GetRingPoint(int index, int ringIndex)
    {
        Vector3 middleOfPerimeter = GetMiddleOfPerimeter();

        float angle = Mathf.Lerp(0f, 2f * Mathf.PI, index / (float)numPoints);
        float x = middleOfPerimeter.x + radius * Mathf.Cos(angle);
        float z = middleOfPerimeter.z + radius * Mathf.Sin(angle);

        //return new Vector3(x, 0, z);
        return ringsAndPerimeter[ringIndex].GetComponent<LineRenderer>().GetPosition(index);
    }

    void GenerateCircularPerimeter()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.loop = true;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(0f, 2f * Mathf.PI, i / (float)numPoints);
            float x = radius * Mathf.Cos(angle) + Random.Range(-noiseScale, noiseScale);
            float z = radius * Mathf.Sin(angle) + Random.Range(-noiseScale, noiseScale);

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
        points = new Vector3[lineRenderer.positionCount];
        lineRenderer.GetPositions(points);
        Debug.Log(GetMiddleOfPerimeter());
    }

    public Vector3 GetMiddleOfPerimeter()
    {
        Vector3 middle = Vector3.zero;

        foreach (Vector3 point in points)
        {
            middle += point;
        }

        middle /= points.Length;
        if (randomMiddle)
        {
            randomMiddle = false;
            return middle + new Vector3(Random.Range(-middleNoise, middleNoise), 0, Random.Range(-middleNoise, middleNoise));
        }
        return middle;
    }

    void AddRings(int numRings, float radiusDecrement, float noiseDecrement, bool reverseRingOrder)
    {
        int ringIndex = numRings;

        for (int i = 1; i <= numRings; i++)
        {
            float currentRadius;
            float currentNoise;
            radiusDecrement =radiusDecrement + Random.Range(-radiusDecrementNoise, radiusDecrementNoise);
            ringRadiusList.Add(radiusDecrement);
            if (reverseRingOrder)
            {
                currentRadius = (ringIndex-i) * radiusDecrement + radiusDecrement;
                currentNoise = noiseScale -  i * noiseDecrement;
            }
            else
            {
                currentRadius = radius - i * radiusDecrement;
                currentNoise = noiseScale - i * noiseDecrement;
            }

            if(currentNoise < 0) { currentNoise = 0; }
            GameObject gameObject = new GameObject("Ring" + i);
            gameObject.transform.parent = transform;
            LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.loop = true;
            lineRenderer.positionCount = numPoints;
            Vector3 middleOfPerimeter = GetMiddleOfPerimeter();

            for (int j = 0; j < numPoints; j++)
            {
                float theta = Mathf.Lerp(0f, 2f * Mathf.PI, j / (float)numPoints);
                float x = middleOfPerimeter.x + currentRadius * Mathf.Cos(theta) + Random.Range(-currentNoise, currentNoise);
                float z = middleOfPerimeter.z + currentRadius * Mathf.Sin(theta) + Random.Range(-currentNoise, currentNoise);

                lineRenderer.SetPosition(j, new Vector3(x, 0, z));
            }
            ringsAndPerimeter.Add(gameObject);

        }
    }
    #endregion
}
