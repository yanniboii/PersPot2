using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateRoad : MonoBehaviour
{
    public delegate void OnRoadBuild();
    public OnRoadBuild onRoadBuild;

    public int res = 30;
    public float width;

    [SerializeField] bool drawGizmos;
    [Range(0.96f, 1f)]
    public float straightThreshold = 0.96f;

    public bool close = true;

    private float lastThreshold;
    private float lastWidth;
    private float lastRes;

    BezierCurve curve;
    Vector3[] points;
    private bool[] isCurve;
    public Vector3[] rightVertices
    {
        get;
        
        private set;
    }
    public Vector3[] leftVertices
    {
        get;

        private set;
    }
    private void OnEnable()
    {
    }

    private void OnDisable()
    {
    }
    // Start is called before the first frame update
    void Awake()
    {
        curve = GetComponent<BezierCurve>();
        points = curve.GetPoints(res);
        GenerateRoadMesh(close);
        AnalyzeRoad();
        curve.OnBezierPointChanged += updateRoad;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

        if (lastRes != res || lastThreshold != straightThreshold || lastWidth != width)
        {
            curve.NotifyPointChanged();
            lastRes = res;
            lastThreshold = straightThreshold;
            lastWidth = width;
        }
        
    }

    void updateRoad(int a)
    {
        GenerateRoadMesh(close);
        AnalyzeRoad();
    }

    void GenerateRoadMesh(bool close = true)
    {
        points = curve.GetPoints(res);
        rightVertices = new Vector3[points.Length];
        leftVertices = new Vector3[points.Length];
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        int numPoints = points.Length;
        int numVertices = 2 * numPoints;
        int numTriangles = 3 * 2 * (numPoints - 1);

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];
        if (!close) numPoints -= res ;
        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the vertices
            Vector3 point = points[i];
            Vector3 heading = CalculateHeading(points, i);
            Vector3 offset = Quaternion.Euler(0, 90, 0) * heading.normalized * width / 2;
            vertices[2 * i] = point + offset;  // right side
            rightVertices[i] = (point + offset);
            vertices[2 * i + 1] = point - offset; // left side
            leftVertices[ i] = (point - offset);

            // Calculate triangles
            if (i < numPoints - 1)
            {
                int t = 6 * i;
                triangles[t] = 2 * i;
                triangles[t + 1] = 2 * i + 1;
                triangles[t + 2] = 2 * (i + 1);
                triangles[t + 3] = 2 * (i + 1);
                triangles[t + 4] = 2 * i + 1;
                triangles[t + 5] = 2 * (i + 1) + 1;
            }
        }
        vertices[numVertices - 2] = vertices[0];
        vertices[numVertices - 1] = vertices[1];

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    Vector3 CalculateHeading(Vector3[] points, int index)
    {
        if (index > 0 && index < points.Length - 1)
        {
            return (points[index + 1] - points[index - 1]).normalized;
        }
        else if (index == 0)
        {
            return (points[1] - points[0]).normalized;
        }
        else // index == points.Length - 1
        {
            return (points[points.Length - 1] - points[points.Length - 2]).normalized;
        }
    }

    void AnalyzeRoad()
    {
        points = curve.GetPoints(res);
        isCurve = new bool[points.Length];

        for (int i = points.Length - 2; i >= 1; i--)
        {
            Vector3 heading1 = (points[i] - points[i - 1]).normalized;
            Vector3 heading2 = (points[i + 1] - points[i]).normalized;
            float dotProduct = Vector3.Dot(heading1, heading2);
            // If the dot product is below the threshold, it's considered a curve
            isCurve[i] = dotProduct < straightThreshold;
        }

        // Assuming the road is closed, handle the first and last points
        isCurve[0] = isCurve[points.Length - 1] = isCurve[1];
    }

    void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            for (int i = 0; i < points.Length; i++)
            {
                if (isCurve[i])
                {
                    Gizmos.color = Color.red; 
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawSphere(rightVertices[i], 0.1f);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawSphere(leftVertices[i], 0.1f);

                }

                Gizmos.DrawSphere(points[i], 0.1f);
            }
        }
    }
}

