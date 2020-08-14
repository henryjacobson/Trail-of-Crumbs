using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement2 : MonoBehaviour
{
    public bool rotate;
    public Transform around;
    public float speed;

    public Vector3 start;
    public Vector3 rotStart;

    private void OnEnable()
    {
        transform.localPosition = start;
        transform.rotation = Quaternion.Euler(rotStart);
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(around.position, Vector3.up, speed * Time.deltaTime);
    }
}
