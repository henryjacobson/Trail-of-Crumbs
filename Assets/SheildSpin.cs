using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheildSpin : MonoBehaviour
{
    public float speed = 45f;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, speed * Time.deltaTime);
    }
}
