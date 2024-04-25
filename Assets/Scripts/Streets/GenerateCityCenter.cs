using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateCityCenter : MonoBehaviour
{
    public int numPoints = 8;
    public float radius = 50f;
    public float noiseScale = 20f;

    GeneratePerimeter perimeter;
    // Start is called before the first frame update
    void Start()
    {
        if(gameObject.GetComponentInParent<GeneratePerimeter>() != null)perimeter = gameObject.GetComponentInParent<GeneratePerimeter>();

        GenerateCircularCenter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GenerateCircularCenter()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.positionCount = numPoints;
        lineRenderer.loop = true;

        Vector3 pos = perimeter.GetMiddleOfPerimeter();

        for (int i = 0; i < numPoints; i++)
        {
            float angle = Mathf.Lerp(0f, 2f * Mathf.PI, i / (float)numPoints);
            float x = pos.x + radius * Mathf.Cos(angle) + Random.Range(-noiseScale, noiseScale);
            float z = pos.y + radius * Mathf.Sin(angle) + Random.Range(-noiseScale, noiseScale);

            lineRenderer.SetPosition(i, new Vector3(x, 0f, z));
        }
    }
}
