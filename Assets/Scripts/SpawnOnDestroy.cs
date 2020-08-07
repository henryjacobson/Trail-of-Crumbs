using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnOnDestroy : MonoBehaviour
{
    public GameObject objectToSpawn;

    Vector3 startingPosn;
    Quaternion startingRot;

    private void Start()
    {
        startingPosn = transform.position;
        startingRot = objectToSpawn.transform.rotation;
    }

    private void OnDestroy()
    {
        Instantiate(objectToSpawn, startingPosn, startingRot);
    }
}
