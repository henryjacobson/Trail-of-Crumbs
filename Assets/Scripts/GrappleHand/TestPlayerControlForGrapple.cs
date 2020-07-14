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

    bool active;

    void Start()
    {
        this.rb = this.GetComponent<Rigidbody>();
        this.active = true;
    }

    void Update()
    {
        if (this.active)
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
            if (Input.GetKey(KeyCode.UpArrow))
            {
                this.transform.Rotate(this.rotateSpeed * Time.deltaTime, 0, 0);
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                this.transform.Rotate(-this.rotateSpeed * Time.deltaTime, 0, 0);
            }
        }
    }
}
