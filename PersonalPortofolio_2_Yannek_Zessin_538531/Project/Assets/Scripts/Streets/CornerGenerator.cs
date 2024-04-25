using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornerGenerator : MonoBehaviour
{
    [SerializeField] int faces = 1;
    [SerializeField] float radius = 5f;
    [SerializeField] float width = 1.0f;
    [SerializeField] Material material;
    public float angularOffset = 2f;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = new GameObject("corner");
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.GetComponent<MeshFilter>().mesh = GenerateCorner();
        gameObject.GetComponent<MeshRenderer>().sharedMaterial = material;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    Mesh GenerateCorner()
    {
        Vector3[] vertices = new Vector3[faces/2];
        int[] triangles = new int[((faces - 2)*3)/2];
        // 3 face = 3 triangles
        // 4 face = 6 triangles
        // 5 face = 9 triangles
        Mesh mesh = new Mesh();


        float angularStep = ((2*Mathf.PI) / (float)(faces) / width);
        vertices[0] = new Vector3(0,0,-5);

        for (int i = 1; i < faces/2; i++)
        {
            float x = radius * Mathf.Cos(i * angularStep*angularOffset);
            float z = radius * Mathf.Sin(i * angularStep* angularOffset);

            vertices[i] = new Vector3(x, 0, z) ;
            Debug.Log(vertices[i]);

        }
        for (int i = 0; i < faces/2 - 2; i++)
        {
            triangles[i*3] = 0;
            triangles[i*3 + 1] = i+2;
            triangles[i*3 + 2] = i+1;
        }


        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
    //private void OnDrawGizmos()
    //{
    //    Vector3[] vertices = new Vector3[faces + 2];
    //    float angularStep = 2f * Mathf.PI / (float)(faces + 2);

    //    for (int i = 0; i < faces + 2; i++)
    //    {
    //        float x = Mathf.Cos(i * angularStep);
    //        float z = Mathf.Sin(i * angularStep);
    //        vertices[i] = new Vector3(x, 0, z) + vertices[0];

    //    }
    //    for(int i = 0; i < faces; i++)
    //    {
    //        Gizmos.DrawCube(vertices[0], Vector3.one);
    //        Gizmos.DrawCube(vertices[i+2], Vector3.one);
    //        Gizmos.DrawCube(vertices[i+1], Vector3.one);
    //    }
    //}
}
