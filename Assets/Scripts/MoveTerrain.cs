using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTerrain : MonoBehaviour
{
    public float speed = 10f;
    public GameObject terrainPrefab;
    public GameObject initialFirst;
    public GameObject initialNext;
    public float zPos = 500f;

    GameObject first;
    GameObject next;
    // Start is called before the first frame update
    void Start()
    {
        first = initialFirst;
        next = initialNext;
    }

    // Update is called once per frame
    void Update()
    {
        first.transform.localPosition += Vector3.back * speed * Time.deltaTime;
        next.transform.localPosition += Vector3.back * speed * Time.deltaTime;

        if (next.transform.localPosition.z <= zPos)
        {
            Destroy(first);
            first = next;
            next = Instantiate(terrainPrefab, first.transform.position + (Vector3.forward * 1000), transform.rotation);
            next.transform.rotation = Quaternion.Euler(90, 90, 0);
            next.transform.SetParent(gameObject.transform);
        }
    }
}
