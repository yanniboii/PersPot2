using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UIElements;

public class GenerateCube : MonoBehaviour
{
    [SerializeField] int pointAmount;
    ConnectionPoints connectionPoints;
    // Start is called before the first frame update
    void Start()
    {
        connectionPoints = new ConnectionPoints();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GenerateFloor(Vector3 position, Vector3 scale, GameObject parent, Material mat, out ConnectionPoints connectionPoints, Quaternion rotation = new Quaternion())
    {
        connectionPoints = new ConnectionPoints();
        GameObject cube = new GameObject("Cube");
        cube.transform.localScale = scale;
        //cube.AddComponent<HouseCollider>();
        cube.layer = LayerMask.NameToLayer("House");
        cube.transform.SetParent(parent.transform, true);
        //BoxCollider coll = cube.AddComponent<BoxCollider>();
        //coll.size = scale;
        //coll.center = new Vector3(scale.x / 2, scale.y / 2, -scale.z / 2);
        //coll.excludeLayers = LayerMask.GetMask("Default");
        //coll.includeLayers = LayerMask.GetMask("House");
        //Rigidbody rb = cube.AddComponent<Rigidbody>();
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        cube.transform.position = position;
        cube.transform.rotation = rotation;
        MeshFilter meshFilter = cube.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = cube.AddComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = mat;

        Mesh cubeMesh = new Mesh();
        float scaleX = scale.x;
        float scaleY = scale.y;
        float scaleZ = scale.z;

        Vector3[] vertices = {
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),

            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),
            new Vector3(0, 1, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),

            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, -1),
        };
        int[] triangles = {
            0, 1, 2, // front
			1, 3, 2,
            4, 6, 7, // back
			5, 4, 7,
            11, 8, 9, //top
			11, 10 ,8,
            12, 14, 15, //bottom
			12, 15, 13,
            16, 19,17,// left
			19, 16, 18,
            20, 21, 23,//right
			23, 22, 20
        };


        Vector2[] uvs = {
    new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),

        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        };

        cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.uv = uvs;
        cubeMesh.Optimize();
        cubeMesh.RecalculateNormals();
        //for (int x = 0; x < pointAmount; x++)
        //{
        //    for (int y = 0; y < pointAmount; y++)
        //    {
        //        ConnectionPoint connectionPoint = new ConnectionPoint();
        //        connectionPoint.point = new Vector3(0 + (scale.x / (pointAmount - 1)) * x, scale.y, 0 + -(scale.z / (pointAmount - 1)) * y);
        //        connectionPoint.connectionType = ConnectionType.wall;
        //        connectionPoints.points.Add(connectionPoint);
        //    }
        //}

        ConnectionPoint cp1 = new ConnectionPoint();
        cp1.point = new Vector3(0, scale.y, 0);
        cp1.rotation = Quaternion.Euler(0, 0, 0);
        cp1.connectionType = ConnectionType.wall;
        cp1.point += parent.transform.position;
        connectionPoints.points.Add(cp1);

        ConnectionPoint cp2 = new ConnectionPoint();
        cp2.point = new Vector3(scale.x, scale.y, 0);
        cp2.rotation =Quaternion.Euler(0, 90, 0);
        cp2.connectionType = ConnectionType.wall;
        cp2.point += parent.transform.position;
        connectionPoints.points.Add(cp2);

        ConnectionPoint cp3 = new ConnectionPoint();
        cp3.point = new Vector3(0, scale.y, -scale.z + 1);
        cp3.rotation = Quaternion.Euler(0, 0, 0);
        cp3.connectionType = ConnectionType.wall;
        cp3.point += parent.transform.position;
        connectionPoints.points.Add(cp3);

        ConnectionPoint cp4 = new ConnectionPoint();
        cp4.point = new Vector3(1, scale.y, 0);
        cp4.rotation = Quaternion.Euler(0, 90, 0);
        cp4.connectionType = ConnectionType.wall;
        cp4.point += parent.transform.position;
        connectionPoints.points.Add(cp4);

        //ConnectionPoint cp5 = new ConnectionPoint();
        //cp5.point = new Vector3(0, 0, 0);
        //cp5.rotation = Quaternion.Euler(0, 0, 0);
        //cp5.connectionType = ConnectionType.ceiling;
        //connectionPoints.points.Add(cp5);

        this.connectionPoints = connectionPoints;
        meshFilter.mesh = cubeMesh;
    }


    public void GenerateWall(Vector3 position, Quaternion rotation, Vector3 scale, GameObject parent, Material mat)
    {
        GameObject cube = new GameObject("Cube");
        cube.transform.localScale = scale;
        cube.transform.SetParent(parent.transform,true);
        cube.layer = LayerMask.NameToLayer("House");
        //cube.AddComponent<HouseCollider>();
        //BoxCollider coll = cube.AddComponent<BoxCollider>();
        //coll.size = scale;
        //coll.center = new Vector3(scale.x/2, scale.y / 2, -scale.z / 2);
        //coll.excludeLayers = LayerMask.GetMask("Default");
        //coll.includeLayers = LayerMask.GetMask("House");
        cube.transform.rotation = rotation;
        cube.transform.position = position;
        //Rigidbody rb = cube.AddComponent<Rigidbody>();
        //rb.isKinematic = true;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        MeshFilter meshFilter = cube.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = cube.AddComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = mat;

        float scaleX = scale.x;
        float scaleY = scale.y;
        float scaleZ = scale.z;

        Mesh cubeMesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(0, 1, 0),
            new Vector3(0, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, 0),

            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),
            new Vector3(0, 1, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 1, 0),
            new Vector3(1, 1, 0),
            new Vector3(0, 1, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),

            new Vector3(1, 0, 0),
            new Vector3(1, 1, 0),
            new Vector3(1, 0, -1),
            new Vector3(1, 1, -1),

            new Vector3(0, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, 0, -1),
            new Vector3(0, 1, -1),
        };
        int[] triangles = {
            0, 1, 2, // front
			1, 3, 2,
            4, 6, 7, // back
			5, 4, 7,
            11, 8, 9, //top
			11, 10 ,8,
            12, 14, 15, //bottom
			12, 15, 13,
            16, 19,17,// left
			19, 16, 18,
            20, 21, 23,//right
			23, 22, 20
        };


        Vector2[] uvs = {
    new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),

        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        };

        cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.uv = uvs;
        cubeMesh.Optimize();
        cubeMesh.RecalculateNormals();
        for (int x = 0; x < pointAmount; x++)
        {
            for (int y = 0; y < pointAmount; y++)
            {
                ConnectionPoint connectionPoint = new ConnectionPoint();
                connectionPoint.point = new Vector3(0 + (scale.x / (pointAmount - 1)) * x, scale.y, 0 + -(scale.z / (pointAmount - 1)) * y);
                connectionPoint.connectionType = ConnectionType.wall;
                //connectionPoints.points.Add(connectionPoint);
            }
        }

        

        meshFilter.mesh = cubeMesh;
    }

    public void GenerateRoofWall(Vector3 position, Quaternion rotation, Vector3 scale, GameObject parent, Material mat)
    {
        GameObject cube = new GameObject("Cube");
        cube.transform.localScale = scale;
        cube.transform.SetParent(parent.transform, true);
        cube.layer = LayerMask.NameToLayer("House");
        //cube.AddComponent<HouseCollider>();
        //BoxCollider coll = cube.AddComponent<BoxCollider>();
        //coll.size = scale;
        //coll.center = new Vector3(scale.x/2, scale.y / 2, -scale.z / 2);
        //coll.excludeLayers = LayerMask.GetMask("Default");
        //coll.includeLayers = LayerMask.GetMask("House");
        cube.transform.rotation = rotation;
        cube.transform.position = position;
        //Rigidbody rb = cube.AddComponent<Rigidbody>();
        //rb.isKinematic = true;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
        MeshFilter meshFilter = cube.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = cube.AddComponent<MeshRenderer>();

        meshRenderer.sharedMaterial = mat;

        float scaleX = scale.x;
        float scaleY = scale.y;
        float scaleZ = scale.z;

        Mesh cubeMesh = new Mesh();

        Vector3[] vertices = {
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0, 0, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(1, 0, 0),

            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),
            new Vector3(0.5f, 0.5f, -1),
            new Vector3(0.5f, 0.5f, -1),

            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0.5f, 0.5f, -1),
            new Vector3(0.5f, 0.5f, -1),

            new Vector3(0, 0, 0),
            new Vector3(1, 0, 0),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, -1),

            new Vector3(1, 0, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(1, 0, -1),
            new Vector3(0.5f, 0.5f, -1),

            new Vector3(0, 0, 0),
            new Vector3(0.5f, 0.5f, 0),
            new Vector3(0, 0, -1),
            new Vector3(0.5f, 0.5f, -1),
        };
        int[] triangles = {
            0, 1, 2, // front
			1, 3, 2,
            4, 6, 7, // back
			5, 4, 7,
            11, 8, 9, //top
			11, 10 ,8,
            12, 14, 15, //bottom
			12, 15, 13,
            16, 19,17,// left
			19, 16, 18,
            20, 21, 23,//right
			23, 22, 20
        };


        Vector2[] uvs = {
    new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),

        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        new Vector2(0, 0),
    new Vector2(0, 1 * scaleY),
    new Vector2(1 * scaleX, 0),
    new Vector2(1 * scaleX, 1 * scaleY),
        };

        cubeMesh.Clear();
        cubeMesh.vertices = vertices;
        cubeMesh.triangles = triangles;
        cubeMesh.uv = uvs;
        cubeMesh.Optimize();
        cubeMesh.RecalculateNormals();
        for (int x = 0; x < pointAmount; x++)
        {
            for (int y = 0; y < pointAmount; y++)
            {
                ConnectionPoint connectionPoint = new ConnectionPoint();
                connectionPoint.point = new Vector3(0 + (scale.x / (pointAmount - 1)) * x, scale.y, 0 + -(scale.z / (pointAmount - 1)) * y);
                connectionPoint.connectionType = ConnectionType.wall;
                //connectionPoints.points.Add(connectionPoint);
            }
        }



        meshFilter.mesh = cubeMesh;
    }
    private void OnDrawGizmos()
    {
        if (connectionPoints != null)
        {
            if (connectionPoints.points != null)
            {
                if (connectionPoints.points.Count != 0)
                {
                    for (int i = 0; i < connectionPoints.points.Count; i++)
                    {
                        Gizmos.DrawSphere(connectionPoints.points[i].point, 0.2f);
                    }
                }
            }
        }

    }
}

public class ConnectionPoints
{
    public List<ConnectionPoint> points = new List<ConnectionPoint>();
}

public struct ConnectionPoint
{
    public Vector3 point;
    public Quaternion rotation;
    public ConnectionType connectionType;
}

public enum ConnectionType
{
    wall,
    ground,
    ceiling
}