using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class GeneratePlot : MonoBehaviour
{
    [SerializeField] public int res;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void Generate(Vector3[] roadVertices)
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        meshFilter.mesh = mesh;

        int numPoints = roadVertices.Length;
        int numVertices = 2 * numPoints;
        int numTriangles = 3 * 2 * (numPoints - 1);

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numTriangles];

        for (int i = 0; i < numPoints; i++)
        {
            // Calculate the vertices
            Vector3 point = roadVertices[i];

            vertices[2 * i] = point;  // right side
            vertices[2 * i + 1] = point + new Vector3(20,20,20); // left side

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
}
