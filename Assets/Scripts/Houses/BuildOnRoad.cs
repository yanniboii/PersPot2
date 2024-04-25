using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuildOnRoad : MonoBehaviour
{
    public delegate void AfterHouseBuild();
    public static AfterHouseBuild afterRoadBuild;

    [SerializeField] public GameObject houseGen;
    [SerializeField] public GenerateRoad generateRoad;

    private int index = 0;

    private void OnEnable()
    {
        HouseCollider.onHouseBuild += HandleHouseBuild;
    }

    private void OnDisable()
    {
        HouseCollider.onHouseBuild -= HandleHouseBuild;
        generateRoad.onRoadBuild -= PlaceHouse;
    }

    private void Awake()
    {

    }
    // Start is called before the first frame update
    void Start()
    {
        generateRoad.onRoadBuild += PlaceHouse;
        StartCoroutine(DelayedRoadBuild());
    }

    // Update is called once per frame
    void Update()
    {

    }

    void PlaceHouse()
    {
        float dist = 0;
        for (int i = 5; i < generateRoad.rightVertices.Length-6; i++)
        {
            dist += (generateRoad.rightVertices[i] - generateRoad.rightVertices[i - 1]).magnitude;
            index = i;
            if(dist  > 30)
            {
                Vector3 direction = generateRoad.rightVertices[i] - generateRoad.leftVertices[i];
                Vector3 _direction = generateRoad.leftVertices[i] - generateRoad.rightVertices[i];
                Vector3 offset = direction.magnitude * new Vector3(0, 0, 2);
                Debug.Log(generateRoad.rightVertices[i] + "  " + generateRoad.leftVertices[i]);
                Quaternion quaternion = Quaternion.LookRotation(direction);
                Quaternion reversed = Quaternion.LookRotation(_direction);
                GameObject house = Instantiate(houseGen, generateRoad.leftVertices[i], quaternion);
                GameObject house2 = Instantiate(houseGen, generateRoad.rightVertices[i], reversed);
                dist = 0;
            }
            if ((i %80) == 5)
            {

            }
        }
    }

    void HandleHouseBuild(HouseCollider house)
    {
        house.elapsedTime = index++;
    }
    IEnumerator DelayedRoadBuild()
    {
        yield return new WaitForSeconds(0.1f);
        generateRoad.onRoadBuild?.Invoke();
        StartCoroutine(RemoveColliders());
    }

    IEnumerator RemoveColliders()
    {
        yield return new WaitForSeconds(0.1f);
        afterRoadBuild?.Invoke();
    }
}
