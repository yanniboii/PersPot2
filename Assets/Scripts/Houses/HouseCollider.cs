using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseCollider : MonoBehaviour
{

    public delegate void OnHouseBuild(HouseCollider houseCollider);
    public static OnHouseBuild onHouseBuild;
        public float elapsedTime;

    private void OnDisable()
    {
        BuildOnRoad.afterRoadBuild -= Remove;
    }

    private void Awake()
    {
        onHouseBuild.Invoke(this);
        BuildOnRoad.afterRoadBuild += Remove;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent != transform.parent)
        {
            if(collision.gameObject.GetComponent<HouseCollider>() != null)
            {
                if (collision.gameObject.GetComponent<HouseCollider>().elapsedTime < elapsedTime)
                {
                    //Destroy(collision.gameObject.transform.parent.gameObject);
                }
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
