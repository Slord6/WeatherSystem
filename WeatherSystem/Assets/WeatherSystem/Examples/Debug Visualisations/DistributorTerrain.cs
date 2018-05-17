using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistributorTerrain : Distributor
{
    [SerializeField]
    private Vector3 vectorRange = new Vector3(100,0,100);
    [SerializeField]
    private Vector3 autoScale = new Vector3(1,1,1);
    [SerializeField]
    private float maxRayDist = 500.0f;
    [SerializeField]
    private int objectsPerFrame = 100;

    private GameObject[] spawnedObjects;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Distribute());
	}

    protected override IEnumerator Distribute()
    {
        spawnedObjects = new GameObject[objectCount];
        for (int i = 0; i < objectCount; i++)
        {
            GameObject newObject = GameObject.Instantiate(distributionObject, RandomVectorInRange(), Quaternion.identity);
            newObject.transform.parent = this.transform;
            newObject.transform.localScale = autoScale;
            spawnedObjects[i] = newObject;
            if (i % objectsPerFrame == 0)
            {
                yield return null;
            }
        }
        Debug.Log("All objects spawned, placing...");
        for(int i = 0; i < objectCount; i++)
        {
            RaycastHit hitInfo;
            if (Physics.Raycast(spawnedObjects[i].transform.position, Vector3.down, out hitInfo, maxRayDist))
            {
                spawnedObjects[i].transform.position = hitInfo.point;
            }
            else
            {
                //Debug.Log("Distribution: Didn't hit for object " + i);
                Destroy(spawnedObjects[i]);
            }
        }
    }

    protected override Vector3 RandomVectorInRange()
    {
        Vector3 startPos = transform.position;
        Vector3 vector = new Vector3();
        if (!lockX)
        {
            vector.x = Random.Range(startPos.x - vectorRange.x, startPos.x + vectorRange.x);
        }
        else
        {
            vector.x = startPos.x;
        }

        if (!lockY)
        {
            vector.y = Random.Range(startPos.y - vectorRange.y, startPos.y + vectorRange.y);
        }
        else
        {
            vector.y = startPos.y;
        }

        if (!lockZ)
        {
            vector.z = Random.Range(startPos.z - vectorRange.z, startPos.z + vectorRange.z);
        }
        else
        {
            vector.z = startPos.z;
        }
        return vector;
    }
}
