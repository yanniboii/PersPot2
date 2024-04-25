using UnityEngine;

public class PentagonalDodecahedronGenerator : MonoBehaviour
{
    public float radius = 1.0f;
    [SerializeField]public AnimationCurve curve;
    public Material material;
    public GameObject gameObejct;

    void Start()
    {



    }


    public void GeneratePentagonalDodecahedron(GameObject PentagonalDodecahedron, Vector3 pos, float thiccness)
    {
        PentagonalDodecahedron.transform.position = pos;
        radius = curve.Evaluate(thiccness);
        //PentagonalDodecahedron.transform.SetParent(parent.transform, false);
        Mesh mesh = new Mesh();

        PentagonalDodecahedron.GetComponent<MeshFilter>().mesh = mesh;
        Vector3[] vertices = new Vector3[20];
        int[] triangles = new int[36];
        Vector2[] uvs = new Vector2[20];

        // Dodecahedron vertex positions
        float phi = ((1f + Mathf.Sqrt(5f)) / 2f);

        for(int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x, vertices[i].z);

        }

        vertices[0] = new Vector3(-radius, -radius, -radius);
        vertices[1] = new Vector3(radius, -radius, -radius);
        vertices[2] = new Vector3(radius, -radius, radius);
        vertices[3] = new Vector3(-radius, -radius, radius);
        vertices[4] = new Vector3(-radius, radius, -radius);
        vertices[5] = new Vector3(radius, radius, -radius);
        vertices[6] = new Vector3(radius, radius, radius);
        vertices[7] = new Vector3(-radius, radius, radius);
        vertices[8] = new Vector3(0, -radius / phi, -phi*radius);
        vertices[9] = new Vector3(0, radius / phi, -phi*radius);
        vertices[10] = new Vector3(0, -radius / phi, phi*radius);
        vertices[11] = new Vector3(0, radius / phi, phi * radius);
        vertices[12] = new Vector3(-phi * radius, 0, -radius / phi);
        vertices[13] = new Vector3(phi * radius, 0, -radius / phi);
        vertices[14] = new Vector3(-phi * radius, 0, radius / phi);
        vertices[15] = new Vector3(phi * radius, 0, radius / phi);
        vertices[16] = new Vector3(-radius / phi, -phi * radius, 0);
        vertices[17] = new Vector3(radius / phi, -phi * radius, 0);
        vertices[18] = new Vector3(-radius / phi, phi * radius, 0);
        vertices[19] = new Vector3(radius / phi, phi * radius, 0);

        //for (int i = 0; i < vertices.Length; i++)
        //{
        //    GameObject cube =Instantiate(gameObejct, vertices[i], Quaternion.identity, gameObject.transform);
        //    cube.name = "cube" + i;
        //    cube.SetActive(false);
        //}

        // Dodecahedron triangle indices
        triangles = new int[108];

        //face 1
        triangles[0] = 0; triangles[1] = 4; triangles[2] = 8;
        triangles[3] = 0; triangles[4] = 12; triangles[5] = 4;
        triangles[6] = 8; triangles[7] = 4; triangles[8] = 9;

        //face 2
        triangles[9] = 9; triangles[10] = 4; triangles[11] = 18;
        triangles[12] = 9; triangles[13] = 18; triangles[14] = 5;
        triangles[15] = 5; triangles[16] = 18; triangles[17] = 19;

        // Face 3
        triangles[18] = 8; triangles[19] = 9; triangles[20] = 5;
        triangles[21] = 8; triangles[22] = 5; triangles[23] = 1;
        triangles[24] = 1; triangles[25] = 5; triangles[26] = 13;

        // Face 4
        triangles[27] = 13; triangles[28] = 5; triangles[29] = 19;
        triangles[30] = 13; triangles[31] = 19; triangles[32] = 15;
        triangles[33] = 15; triangles[34] = 19; triangles[35] = 6;

        // Face 5
        triangles[36] = 6; triangles[37] = 19; triangles[38] = 18;
        triangles[39] = 6; triangles[40] = 18; triangles[41] = 11;
        triangles[42] = 11; triangles[43] = 18; triangles[44] = 7;

        // Face 6 
        triangles[45] = 1; triangles[46] = 13; triangles[47] = 15;
        triangles[48] = 17; triangles[49] = 1; triangles[50] = 15;
        triangles[51] = 17; triangles[52] = 15; triangles[53] = 2;

        // Face 7
        triangles[54] = 12; triangles[55] = 18; triangles[56] = 4;
        triangles[57] = 12; triangles[58] = 14; triangles[59] = 18;
        triangles[60] = 14; triangles[61] = 7; triangles[62] = 18;

        // Face 8
        triangles[63] = 0; triangles[64] = 14; triangles[65] = 12;
        triangles[66] = 0; triangles[67] = 16; triangles[68] = 14;
        triangles[69] = 16; triangles[70] = 3; triangles[71] = 14;

        // Face 9
        triangles[72] = 14; triangles[73] = 11; triangles[74] = 7;
        triangles[75] = 14; triangles[76] = 3; triangles[77] = 11;
        triangles[78] = 3; triangles[79] = 10; triangles[80] = 11;

        // Face 10 
        triangles[81] = 17; triangles[82] = 8; triangles[83] = 1;
        triangles[84] = 17; triangles[85] = 16; triangles[86] = 8;
        triangles[87] = 16; triangles[88] = 0; triangles[89] = 8;

        // Face 11 i am here now
        triangles[90] = 17; triangles[91] = 2; triangles[92] = 10;
        triangles[93] = 17; triangles[94] = 10; triangles[95] = 16;
        triangles[96] = 16; triangles[97] = 10; triangles[98] = 3;

        // Face 12
        triangles[99] = 2; triangles[100] = 15; triangles[101] = 6;
        triangles[102] = 2; triangles[103] = 6; triangles[104] = 10;
        triangles[105] = 10; triangles[106] = 6; triangles[107] = 11;

        mesh.vertices = vertices;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        // Recalculate normals to improve lighting
        mesh.RecalculateNormals();
        PentagonalDodecahedron.GetComponent<MeshRenderer>().sharedMaterial = material;
    }


}