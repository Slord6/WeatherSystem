using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Distributor : MonoBehaviour {

    [SerializeField]
    protected float range;
    [SerializeField]
    protected GameObject distributionObject;
    [SerializeField]
    protected int objectCount;
    [SerializeField]
    protected bool lockX = false;
    [SerializeField]
    protected bool lockY = false;
    [SerializeField]
    protected bool lockZ = false;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(Distribute());
	}

    protected virtual IEnumerator Distribute()
    {
        for (int i = 0; i < objectCount; i++)
        {
            GameObject newObject = GameObject.Instantiate(distributionObject, RandomVectorInRange(), Quaternion.identity);
            newObject.transform.parent = this.transform;
            if (i % 10 == 0)
            {
                yield return null;
            }
        }
    }

    protected virtual Vector3 RandomVectorInRange()
    {
        Vector3 vector = new Vector3();
        if (!lockX)
        {
            vector.x = Random.Range(-range, range);
        }
        if (!lockY)
        {
            vector.y = Random.Range(-range, range);
        }
        if (!lockZ)
        {
            vector.z = Random.Range(-range, range);
        }
        return vector;
    }
}
