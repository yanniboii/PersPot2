using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadCollider : MonoBehaviour
{
    private void OnDisable()
    {
        BuildOnRoad.afterRoadBuild -= Remove;
    }


    // Start is called before the first frame update
    void Start()
    {
        BuildOnRoad.afterRoadBuild += Remove;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != transform.parent)
        {
            if (collision.gameObject.GetComponent<HouseCollider>() != null)
            {
                Destroy(collision.gameObject.transform.parent.gameObject);
            }
        }
    }

    void Remove()
    {
        Destroy(GetComponent<Collider>());
        Destroy(GetComponent<Rigidbody>());
        Destroy(this);
    }
}
