using UnityEngine;

public class ConeGen : MonoBehaviour
{
    [SerializeField] int segments = 12;
    [SerializeField] float radius = 5f;
    [SerializeField] float height = 10f;
    [SerializeField] Material material;

    void Start()
    {
        GameObject conePlaneObject = new GameObject("conePlane");
        MeshFilter meshFilter = conePlaneObject.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = conePlaneObject.AddComponent<MeshRenderer>();

        meshFilter.mesh = GenerateConePlane();
        meshRenderer.sharedMaterial = material;
    }

    Mesh GenerateConePlane()
    {
        Vector3[] vertices = new Vector3[segments * 3];
        int[] triangles = new int[segments * 3];
        Mesh mesh = new Mesh();

        float angularStep = 2f * Mathf.PI / (float)(segments);

        for (int i = 0; i < segments; i++)
        {
            float x = Mathf.Cos(i * angularStep) * radius;
            float z = Mathf.Sin(i * angularStep) * radius;

            // Vertex at the center of the cone
            Vector3 centerVertex = new Vector3(0f, 0f, 0f);

            // Vertices forming the plane within the cone shape
            vertices[i * 3] = centerVertex;
            vertices[i * 3 + 1] = new Vector3(x, 0f, z);
            vertices[i * 3 + 2] = new Vector3(x, height, z);

            // Triangle indices
            triangles[i * 3] = i * 3;
            triangles[i * 3 + 1] = i * 3 + 1;
            triangles[i * 3 + 2] = i * 3 + 2;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}   