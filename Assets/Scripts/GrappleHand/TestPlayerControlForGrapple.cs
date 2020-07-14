using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerControlForGrapple : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float rotateSpeed;

    private Rigidbody rb;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(Vector3.down, this.rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(Vector3.up, this.rotateSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + (this.transform.forward * this.speed), this.speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + (this.transform.forward * -this.speed), this.speed * Time.deltaTime);
        }
    }
}
