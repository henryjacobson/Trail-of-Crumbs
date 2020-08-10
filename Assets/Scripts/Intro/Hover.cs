using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    public float hoverAmount = 1f;
    public float hoverSpeed = 1f;
    Vector3 start;
    // Start is called before the first frame update
    void Start()
    {
        start = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (IntroBehavior.intro)
        {
            transform.rotation *= Quaternion.Euler(Vector3.up * 45f * Time.deltaTime);
            transform.position = start + Vector3.up * Mathf.Sin(Time.time * hoverSpeed) * hoverAmount;
        }
    }
}
