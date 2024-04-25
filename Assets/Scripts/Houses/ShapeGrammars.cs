using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

public class HouseTransformInfo
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 offset;
    public int connectionPointIndex;
    public ConnectionPoints connectionPoints;
}

public class ShapeGrammars : MonoBehaviour
{
    [SerializeField] public string axiom;
    [SerializeField] public int recursion = 1;
    [SerializeField] public Vector3 HorizontalWallSize;
    [SerializeField] public Vector3 VerticalWallSize;
    [SerializeField] public Vector3 FloorSize;
    [SerializeField] public Vector3 RoofSize;

    [SerializeField] public Vector3 BuildingBounds;

    [SerializeField] public List<char> letter;
    [SerializeField] public List<string> rule;
    [SerializeField] int north;
    [SerializeField] int east;
    [SerializeField] int south;
    [SerializeField] int west;
    [SerializeField] Material firstFloorMat;
    [SerializeField] Material secondFloorMat;
    [SerializeField] Material FloorMat;
    [SerializeField] Material RoofMat;



    Dictionary<char, string> rules;
    private string currentString = string.Empty;
    private Stack<HouseTransformInfo> transformStack;

    GenerateCube generateCube;

    // Start is called before the first frame update
    void Awake()
    {
        transform.position += -transform.forward * 20f;
        transformStack = new Stack<HouseTransformInfo>();

        if (letter.Count != rule.Count)
        {
            Debug.LogError("The 'letter' and 'rule' lists must have the same length.");
            return;
        }
        rules = letter.Zip(rule, (l, r) => new { Letter = l, Rule = r })
            .ToDictionary(item => item.Letter, item => item.Rule);
        generateCube = GetComponent<GenerateCube>();
        //spawntrees();
        Generate();
    }

    public void Generate()
    {
        currentString = axiom;
        GameObject house = new GameObject("House");
        house.transform.position = transform.position;
        house.AddComponent<MeshRenderer>();
        house.AddComponent<MeshFilter>();

        ConnectionPoints connectionPoints = new ConnectionPoints();
        Vector3 position = Vector3.zero;
        Quaternion rotation = Quaternion.identity;
        Vector3 offset = Vector3.zero;
        int connectionPointIndex = 0;
        bool destroyNextChar = false;
        for (int i = 0; i < recursion; i++)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (char c in currentString)
            {
                
                if (rules.ContainsKey(c))
                {
                    if (destroyNextChar)
                    {
                        rules[c] = rules[c].Substring(0, rules[c].Length - 1);
                        rules.Remove(c);
                        destroyNextChar = false;
                        Debug.Log("DIED");
                    }
                    if (rules[c].Contains("("))
                    {
                        //string substring = rules[c].Substring(7, rules[c].Length-1);
                        string temp = rules[c];
                        string[] rands = temp.Split(',' ,'(',')');
                        string beforeParentheses = temp.Substring(0, temp.IndexOf('('));
                        string afterParentheses = temp.Substring(temp.IndexOf(')') + 1);
                        if(beforeParentheses != null)stringBuilder.Append(beforeParentheses);
                        int rule = Random.Range(1, rands.Length-1);

                        stringBuilder.Append(rands[rule]);
                        if (afterParentheses != null) stringBuilder.Append(afterParentheses);

                    }
                    else
                    {
                        stringBuilder.Append(rules[c]);
                    }
                }
                else if(c == '1') 
                { 
                    destroyNextChar = true;
                }
                else if(c == 'N')
                {
                    if (north > 1)
                    {
                        north--;
                    }
                    if (south > 1)
                    {
                        south--;
                    }
                    if (west > 1)
                    {
                        west--;
                    }
                    if (east > 1)
                    {
                        east--;
                    }

                    for (int j = 0; j < north; j++)
                    {
                        connectionPointIndex = 0;
                        string s = "W+";
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int j = 0; j < east; j++)
                    {
                        connectionPointIndex = 1;
                        string s = "W+";
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int j = 0; j < south; j++)
                    {
                        connectionPointIndex = 2;
                        string s = "W+";
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int j = 0; j < west; j++)
                    {
                        connectionPointIndex = 3;
                        string s = "W+";
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                }
                else
                {
                    stringBuilder.Append(c);
                }
            }
            currentString = stringBuilder.ToString();
        }
        Debug.Log(currentString);

        // Create an array of CombineInstance with the appropriate size
        List<GameObject> oldGameObjects = new List<GameObject>();


        foreach (char c in currentString)
        {
            switch (c)
            {
                case 'F':
                    if(transformStack.Count > 0 && connectionPoints.points.Count > 0 && connectionPointIndex < connectionPoints.points.Count)
                    {
                        generateCube.GenerateFloor(connectionPoints.points[connectionPointIndex].point + offset, FloorSize, house, FloorMat, out connectionPoints);
                    }
                    else
                    {
                        generateCube.GenerateFloor(this.transform.position + offset, FloorSize, house, FloorMat, out connectionPoints);
                    }
                    break;
                case 'C':
                        if (connectionPointIndex == 0)
                        {
                            north++;
                            generateCube.GenerateRoofWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 1)
                        {
                            east++;
                            generateCube.GenerateRoofWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(-0.01f, 0, -0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 2)
                        {
                            south++;
                            generateCube.GenerateRoofWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 3)
                        {
                            west++;
                            generateCube.GenerateRoofWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(0.01f, 0, 0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, secondFloorMat);
                        }
                    
                    break;
                case 'W':
                    if(transformStack.Count > 1)
                    {
                        if (connectionPointIndex == 0)
                        {
                            north++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 1)
                        {
                            east++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(-0.01f, 0, -0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 2)
                        {
                            south++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, secondFloorMat);
                        }
                        if (connectionPointIndex == 3)
                        {
                            west++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(0.01f, 0, 0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, secondFloorMat);
                        }
                    }
                    else
                    {
                        if (connectionPointIndex == 0)
                        {
                            north++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, firstFloorMat);
                        }
                        if (connectionPointIndex == 1)
                        {
                            east++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(-0.01f, 0, -0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, firstFloorMat);
                        }
                        if (connectionPointIndex == 2)
                        {
                            south++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset, connectionPoints.points[connectionPointIndex].rotation, HorizontalWallSize, house, firstFloorMat);
                        }
                        if (connectionPointIndex == 3)
                        {
                            west++;
                            generateCube.GenerateWall(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(0.01f, 0, 0.001f), connectionPoints.points[connectionPointIndex].rotation, VerticalWallSize, house, firstFloorMat);
                        }
                    }

                    break;
                case'N':

                    if(north > 1)
                    {
                        north--;
                    }
                    if(south > 1)
                    { 
                        south--;
                    }
                    if(west > 1)
                    {
                        west--;
                    }
                    if(east > 1)
                    {
                        east--;
                    }

                    for(int i = 0; i < north; i++)
                    {
                        connectionPointIndex = 0;
                        string s = "W+";
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int i = 0; i < east; i++)
                    {
                        connectionPointIndex = 1;
                        string s = "W+";
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int i = 0; i < south; i++)
                    {
                        connectionPointIndex = 2;
                        string s = "W+";
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    for (int i = 0; i < west; i++)
                    {
                        connectionPointIndex = 3;
                        string s = "W+";
                        StringBuilder stringBuilder = new StringBuilder();
                        stringBuilder.Append(s);
                        currentString += stringBuilder.ToString();
                    }
                    Debug.Log(currentString);
                    break;
                case 'O':
                    if (connectionPointIndex == 0)
                    {
                        north++;
                    }
                    if (connectionPointIndex == 1)
                    {
                        east++;
                    }
                    if (connectionPointIndex == 2)
                    {
                        south++;
                    }
                    if (connectionPointIndex == 3)
                    {
                        west++;
                    }
                    break;
                case 'R':
                    if (connectionPointIndex == 0)
                    {
                        generateCube.GenerateFloor(connectionPoints.points[connectionPointIndex].point + offset, RoofSize, house, RoofMat, out connectionPoints, Quaternion.Euler(45,0,0));
                    }
                    if (connectionPointIndex == 1)
                    {
                        generateCube.GenerateFloor(connectionPoints.points[connectionPointIndex].point + offset + new Vector3(-5, 0, 0), RoofSize, house, RoofMat, out connectionPoints, Quaternion.Euler(0, 0, 45));
                    }
                    if (connectionPointIndex == 2)
                    {
                        generateCube.GenerateFloor(connectionPoints.points[connectionPointIndex].point + offset +new Vector3(5.0001f,0,-1), RoofSize + new Vector3(-0.001f,0,0), house, RoofMat, out connectionPoints, Quaternion.Euler(45, 180, 0));
                    }
                    if (connectionPointIndex == 3)
                    {
                        generateCube.GenerateFloor(connectionPoints.points[connectionPointIndex].point + offset+new Vector3(4,0,-5), RoofSize, house, RoofMat, out connectionPoints, Quaternion.Euler(0, 180, 45));
                    }
                    break;
                case '[':
                    transformStack.Push(new HouseTransformInfo()
                    {
                        position = position,
                        rotation = rotation,
                        offset = offset,
                        connectionPointIndex = connectionPointIndex,
                        connectionPoints = connectionPoints
                    }) ;
                    break;
                case ']':
                    HouseTransformInfo hti = transformStack.Pop();
                    position = hti.position;
                    rotation = hti.rotation;
                    offset = hti.offset;
                    connectionPointIndex = hti.connectionPointIndex;
                    connectionPoints = hti.connectionPoints;
                    break;
                case '>':
                    if (connectionPoints.points.Count > connectionPointIndex)
                    {
                        connectionPointIndex ++;
                    }
                    else
                    {
                        if (!(connectionPoints.points.Count >= connectionPointIndex-1))
                        {
                            connectionPointIndex = 0;
                        }
                    }
                    Quaternion rotation2 = Quaternion.Euler(0,90,0);
                    //rotation*= rotation2;
                    break;
                case '<':
                    if (connectionPoints.points.Count > connectionPointIndex)
                    {
                        connectionPointIndex++;
                    }
                    else
                    {
                        if (!(connectionPoints.points.Count >= connectionPointIndex - 1))
                        {
                            connectionPointIndex = 0;
                        }
                    }
                    Quaternion rotation3 = Quaternion.Euler(0, -90, 0);
                    rotation *= rotation3; 
                    break;
                case '^':
                    offset.y += HorizontalWallSize.y;
                    break;
                case '+':
                    offset.x += FloorSize.x;
                    break;
                case '-':
                    offset.x -= FloorSize.x;
                    break;
                case '|':
                    offset.z += FloorSize.z;
                    break;
                case '/':
                    FloorSize.x /= 2;
                    FloorSize.z /= 2;
                    HorizontalWallSize.x /= 2;
                    HorizontalWallSize.y /= 2;
                    VerticalWallSize.x /= 2;
                    VerticalWallSize.y /= 2;
                    break;
                case '*':
                    FloorSize.x *= 2;
                    FloorSize.z *= 2;
                    HorizontalWallSize.x *= 2;
                    HorizontalWallSize.y *= 2;
                    VerticalWallSize.x *= 2;
                    VerticalWallSize.y *= 2;
                    break;
                case ',':
                    break;
                case '.':
                    break;
            }

        }
        foreach (var oldGameObject in oldGameObjects)
        {
            Destroy(oldGameObject);
        }

        // Create separate meshes for branches and leaves


        // Assign the branch and leaf meshes to the tree's MeshFilter

        // Set the material for branches
        house.GetComponent<MeshRenderer>().material.SetVector("_Vector2", new Vector2(Random.Range(0f, 1f), Random.Range(0f, 1f)));
        float randomFloat = Random.Range(0f, 1f);
        float randomFloat2 = Random.Range(0f, 1f);

        house.GetComponent<MeshRenderer>().material.SetFloat("_Float", randomFloat);
        //tree.GetComponent<MeshFilter>().sharedMesh = MeshSmoothener.SmoothMesh(tree.GetComponent<MeshFilter>().sharedMesh, 1, MeshSmoothener.Filter.Laplacian);
        // Optionally, you can create a separate GameObject for leaves if needed


        // Set the material for leaves
        house.transform.rotation = transform.rotation;
    }

    private void OnDrawGizmos()
    {
        
    }
}
