using System.Collections.Generic;
using UnityEngine;

public class TreeBranchConnector : MonoBehaviour
{
    public List<GameObject> branches; // List of branch game objects

    public float maxConnectionDistance = 1.0f; // Maximum distance for branches to connect

    void Start()
    {
        ConnectBranches();
    }

    void ConnectBranches()
    {
        for (int i = 0; i < branches.Count; i++)
        {
            for (int j = i + 1; j < branches.Count; j++)
            {
                GameObject branchA = branches[i];
                GameObject branchB = branches[j];

                Vector3 posA = branchA.transform.position;
                Vector3 posB = branchB.transform.position;

                float distance = Vector3.Distance(posA, posB);

                if (distance <= maxConnectionDistance)
                {
                    // Calculate a point in between the two branch positions
                    Vector3 connectionPoint = (posA + posB) / 2f;

                    // Create a connector (a small cylinder or similar object) between the branches
                    GameObject connector = CreateConnector(posA, posB, connectionPoint);

                    // Parent the connector to one of the branches
                    connector.transform.parent = branchA.transform;

                    // Note: You may need to adjust the connector's orientation and scaling

                    // Destroy or disable the original branch objects if needed
                    // Destroy(branchA);
                    // Destroy(branchB);
                }
            }
        }
    }

    GameObject CreateConnector(Vector3 start, Vector3 end, Vector3 position)
    {
        GameObject connector = GameObject.CreatePrimitive(PrimitiveType.Cylinder);

        // Set the position and scale of the connector
        connector.transform.position = position;
        connector.transform.localScale = new Vector3(0.1f, 0.1f, Vector3.Distance(start, end));

        // You may need to adjust the orientation of the connector

        return connector;
    }
}
