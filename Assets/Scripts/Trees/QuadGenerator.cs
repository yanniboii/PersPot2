using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadGenerator : MonoBehaviour
{
    [SerializeField] Material material;
    public float width = 1.0f;
    public float height = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        GenerateQuad();
    }

    void GenerateQuad()
    {
        GameObject quadObject = new GameObject("Quad");

        MeshRenderer quadRenderer = quadObject.AddComponent<MeshRenderer>();

        Mesh quadMesh = new Mesh();

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(-width / 2, 0, height / 2);
        vertices[1] = new Vector3(width / 2, 0, height / 2);
        vertices[2] = new Vector3(width / 2, 0, -height / 2);
        vertices[3] = new Vector3(-width / 2, 0, -height / 2);

        int[] triangles = new int[6] { 0, 1, 2, 0, 2, 3 };

        Vector2[] uv = new Vector2[4];
        uv[0] = new Vector2(0, 0);
        uv[1] = new Vector2(1, 0);
        uv[2] = new Vector2(1, 1);
        uv[3] = new Vector2(0, 1);

        quadMesh.vertices = vertices;
        quadMesh.triangles = triangles;
        quadMesh.uv = uv;

        MeshFilter meshFilter = quadObject.AddComponent<MeshFilter>();
        meshFilter.mesh = quadMesh;
        quadRenderer.sharedMaterial = material;
    }
}
